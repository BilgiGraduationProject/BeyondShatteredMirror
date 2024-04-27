using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections.Generic;
using Runtime.Signals;
using Runtime.Enums.GameManager;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.Playable;
using Runtime.Enums.Pool;
using Runtime.Utilities;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Runtime.Controllers
{
    public class CutsceneController : MonoBehaviour
    {
        #region Self Variables

        #region Serializable Variables
        
        [SerializeField] private GameObject blackwBG;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Button completeButton;
        
        #endregion
        
        #region Private Variables
    
        private int _lastIndex;
        private string _fullPath;
        private int _index;

        #endregion

        #endregion
        
        private void Awake()
        {
            blackwBG.SetActive(false);
            // foreach (var go in cutsceneList)
            // {
            //     go.GetComponent<VideoPlayer>().Prepare();
            //     go.SetActive(false);
            // }
        }

        #region SubscribeEvents and UnsubscribeEvents

        private void OnEnable() => SubscribeEvents();
    
        private void SubscribeEvents()
        {
            //SceneManager.sceneLoaded += OnSceneLoaded;
            CoreUISignals.Instance.onOpenCutscene += OnOpenCutscene;
        }

        private void UnsubscribeEvents()
        {
            //SceneManager.sceneLoaded -= OnSceneLoaded;
            CoreUISignals.Instance.onOpenCutscene -= OnOpenCutscene;
        }
    
        private void OnDisable() => UnsubscribeEvents();
    
        #endregion

        private void Update()
        {
            // TODO: This part is for testing purposes only. Change it later with collision or something else.
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                //CoreUISignals.Instance.onOpenCutscene?.Invoke(0); // This method helps to load cutscene video.
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //CoreUISignals.Instance.onOpenCutscene?.Invoke(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //CoreUISignals.Instance.onOpenCutscene?.Invoke(2);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                CompleteButton();
            }
        }
        
        private void OnOpenCutscene(int index)
        {
            LoadVideoClip(index);
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
            completeButton.gameObject.SetActive(true);
            _index = index - 1;
            switch (_index)
            {
                case (int)PlayableEnum.BathroomLayingSeize:
                    PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.BathroomLayingSeize);
                    break;
                case (int)PlayableEnum.EnteredHouse:
                    PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.EnteredHouse);
                    break;
                
            }
        }
        
        private void LoadVideoClip(int index)
        {
            try
            {
                _fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, $"Cutscenes/Cutscene{index}.mp4");

                if (!System.IO.File.Exists(_fullPath))
                {
                    throw new System.IO.FileNotFoundException($"Cutscene{index}.mp4 couldn't found.");
                }
                
                //CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
                CoreUISignals.Instance.onDisableAllPanels?.Invoke();
                
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                if (_index is 0)
                {
                    videoPlayer.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 0).SetEase(Ease.OutQuad);
                }
                else
                {
                    videoPlayer.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 0.75f).SetEase(Ease.OutQuad);
                }
                
                videoPlayer.gameObject.GetComponent<VideoPlayer>().loopPointReached += OnCutsceneCompleted;
                
                videoPlayer.url = _fullPath;
                videoPlayer.Prepare();
                
                Debug.Log("Video is playing.".ColoredText(Color.green));
            }
            catch (System.Exception ex)
            {
                Debug.Log($"While uploading an error occurs: {ex.Message}".ColoredText(Color.blue));
                videoPlayer.gameObject.SetActive(false);
                videoPlayer.GetComponent<CanvasGroup>().alpha = 1;
            }
        }

        public void CompleteButton()
        {
            videoPlayer.Stop();
            completeButton.gameObject.SetActive(false);
            OnCutsceneCompleted(videoPlayer);
        }
        
        private void OnCutsceneCompleted(VideoPlayer videoPlayerr)
        {
            print("This is working.");
            videoPlayerr.loopPointReached -= OnCutsceneCompleted;
            blackwBG.SetActive(true);
            
            videoPlayer.GetComponent<CanvasGroup>().DOFade(0f, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
               
                videoPlayer.gameObject.SetActive(false);
                videoPlayer.GetComponent<CanvasGroup>().alpha = 1;
                videoPlayer.GetComponent<VideoPlayer>().Prepare(); // Unnecessary line of code.
                switch (_index)
                {
                    case (int)PlayableEnum.BathroomLayingSeize:
                        PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.BathroomLayingSeize);
                        break;
                    case (int)PlayableEnum.EnteredHouse:
                        PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.EnteredHouse);
                        break;
                    
                }
                blackwBG.GetComponent<CanvasGroup>().DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                   
                    blackwBG.SetActive(false);
                    blackwBG.GetComponent<CanvasGroup>().alpha = 1;
                    
                   
                    CoreUISignals.Instance.onEnableAllPanels?.Invoke();
                });
            });
        }
    }
}