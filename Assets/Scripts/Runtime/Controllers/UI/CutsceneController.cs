using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections.Generic;
using Runtime.Signals;
using Runtime.Enums.GameManager;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Utilities;

namespace Runtime.Controllers
{
    public class CutsceneController : MonoBehaviour
    {
        #region Self Variables

        #region Serializable Variables
        
        [SerializeField] private GameObject blackwBG;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private List<GameObject> cutsceneList = new List<GameObject>();
        
        #endregion
        
        #region Private Variables
    
        private int _lastIndex;
        private string _fullPath;

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
        }
    
        private void OnDisable() => UnsubscribeEvents();
    
        #endregion

        private void Update()
        {
            // TODO: This part is for testing purposes only. Change it later with collision or something else.
            if (Input.GetKeyDown(KeyCode.V))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(0); // This method helps to load cutscene video.
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(1);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(2);
            }
        }
        
        private void OnOpenCutscene(int index)
        {
            LoadVideoClip(index);

            // _lastIndex = index;
            // CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.CancelPlayerMovement);
            // blackwBG.SetActive(false);
            // cutsceneList[index].SetActive(true);
            // cutsceneList[index].GetComponent<VideoPlayer>().Play();
            // cutsceneList[index].GetComponent<CanvasGroup>().alpha = 0;
            // cutsceneList[index].GetComponent<CanvasGroup>().DOFade(1f, .75f).SetEase(Ease.OutQuad);
            // cutsceneList[index].GetComponent<VideoPlayer>().loopPointReached += OnCutsceneFinished;
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
                
                CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Camera);
                
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                videoPlayer.gameObject.GetComponent<CanvasGroup>().DOFade(1f, .75f).SetEase(Ease.OutQuad);
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
        
        private void OnCutsceneFinished(VideoPlayer videoPlayer)
        {
            videoPlayer.loopPointReached -= OnCutsceneFinished;
            blackwBG.SetActive(true);
            // Tween tween = cutsceneList[_lastIndex].GetComponent<CanvasGroup>().DOFade(0, 3f);
            // tween.onComplete += () =>
            // {
            //     cutsceneList[_lastIndex].GetComponent<CanvasGroup>().alpha = 1;
            //     blackwBG.SetActive(false);
            //     cutsceneList[_lastIndex].SetActive(false);
            //     CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StartPlayer);
            // };
            cutsceneList[_lastIndex].GetComponent<CanvasGroup>().DOFade(0f, 3).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                cutsceneList[_lastIndex].GetComponent<CanvasGroup>().alpha = 1;
                cutsceneList[_lastIndex].SetActive(false);
                cutsceneList[_lastIndex].GetComponent<VideoPlayer>().Prepare();
                blackwBG.GetComponent<CanvasGroup>().DOFade(0f, 3f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    blackwBG.GetComponent<CanvasGroup>().alpha = 1;
                    blackwBG.SetActive(false);
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
                });
            });
        }
        
        private void OnCutsceneCompleted(VideoPlayer videoPlayerr)
        {
            videoPlayerr.loopPointReached -= OnCutsceneCompleted;
            blackwBG.SetActive(true);
            
            videoPlayer.GetComponent<CanvasGroup>().DOFade(0f, 3).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                videoPlayer.gameObject.SetActive(false);
                videoPlayer.GetComponent<CanvasGroup>().alpha = 1;
                videoPlayer.GetComponent<VideoPlayer>().Prepare(); // Unnecessary line of code.
                blackwBG.GetComponent<CanvasGroup>().DOFade(0f, 3f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    blackwBG.SetActive(false);
                    blackwBG.GetComponent<CanvasGroup>().alpha = 1;
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
                });
            });
        }
    }
}