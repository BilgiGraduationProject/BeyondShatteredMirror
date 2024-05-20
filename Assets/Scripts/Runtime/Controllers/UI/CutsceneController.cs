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
using Runtime.Enums.UI;
using Runtime.Managers;
using Runtime.Utilities;
using Sirenix.OdinInspector;
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
            CoreUISignals.Instance.onOpenUnCutScene += OnOpenUnCutScene;
            CoreUISignals.Instance.onCloseUnCutScene += OnCloseUnCutScene;
            videoPlayer.started += OnVideoPlayerStarted;
        }

        private void OnVideoPlayerStarted(VideoPlayer source)
        {
            switch (_index)
            {
                case 0:
                    PoolSignals.Instance.onLoadLevel?.Invoke(LevelEnum.LevelAslanHouse,PlayableEnum.BathroomLayingSeize);
                    CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.BathroomLayingSeize);
                    break;
                
                case 1:
                    PoolSignals.Instance.onDestroyTheCurrentLevel?.Invoke();
                    PoolSignals.Instance.onSetAslanHouseVisible?.Invoke(false);
                    PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.EnteredHouse);
                    break;
                case 2:
                    PoolSignals.Instance.onLoadLevel?.Invoke(LevelEnum.LevelMansion,PlayableEnum.Mansion);
                    PoolSignals.Instance.onSetAslanHouseVisible?.Invoke(true);
                    break;
                
            }
        }

        private void UnsubscribeEvents()
        {
            //SceneManager.sceneLoaded -= OnSceneLoaded;
            CoreUISignals.Instance.onOpenCutscene -= OnOpenCutscene;
            CoreUISignals.Instance.onOpenUnCutScene -= OnOpenUnCutScene;
            CoreUISignals.Instance.onCloseUnCutScene -= OnCloseUnCutScene;
            videoPlayer.started -= OnVideoPlayerStarted;
        }

       

        private void OnDisable() => UnsubscribeEvents();
    
        #endregion
        
        private void OnOpenCutscene(int index)
        {
            _index = index-1;
            LoadVideoClip(index);
            CoreUISignals.Instance.onStopMusic?.Invoke();
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
            DOVirtual.DelayedCall(3f ,() =>
            {
                completeButton.gameObject.SetActive(true);
            });
            
            
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
                completeButton.gameObject.SetActive(false);
                videoPlayer.gameObject.SetActive(false);
                switch (_index)
                {
                    case 0: // Game Start, Go to House
                        Debug.LogWarning("Playing seizing");
                        PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.BathroomLayingSeize);
                        CoreUISignals.Instance.onPlayMusic?.Invoke(SFXTypes.AslanHouse);
                        break;
                    case 1: // This is entered house
                        PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.EnteredHouse);
                        CoreUISignals.Instance.onPlayMusic?.Invoke(SFXTypes.AslanHouse);
                        CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.EnteredHouse);
                        break;
                    case 2: // This is mansion
                        PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.Mansion);
                        CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.Mansion);
                        CoreUISignals.Instance.onPlayMusic?.Invoke(SFXTypes.Mansion);
                        break;
                    case 3: // After House, Go to Mansion
                       
                        break;
                }
                videoPlayer.GetComponent<CanvasGroup>().alpha = 1;
                videoPlayer.GetComponent<VideoPlayer>().Prepare(); // Unnecessary line of code.
               
                blackwBG.GetComponent<CanvasGroup>().DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    
                    blackwBG.SetActive(false);
                    blackwBG.GetComponent<CanvasGroup>().alpha = 1;
                   
                  
                });
            });
        }
        
        
        
        
        [Button("Open Cutscene")]
        private void OnOpenUnCutScene(PlayableEnum playableEnum)
        {
            blackwBG.GetComponent<CanvasGroup>().alpha = 0;
            blackwBG.SetActive(true);
            blackwBG.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                switch (playableEnum)
                {
                    case PlayableEnum.StandFrontOfMirror:
                        PoolSignals.Instance.onLoadLevel?.Invoke(LevelEnum.LevelFactory,PlayableEnum.EnteredFactory);
                        CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.EnteredFactory);
                        PoolSignals.Instance.onSetAslanHouseVisible?.Invoke(true);
                        CoreUISignals.Instance.onPlayMusic?.Invoke(SFXTypes.FactoryWhispers);
                        break;
                    case PlayableEnum.SpawnPoint:
                        PlayerSignals.Instance.onPlayerSaveTransform?.Invoke();
                        PoolSignals.Instance.onSetCurrentLevelToVisible?.Invoke(false);
                        PoolSignals.Instance.onLoadLevel?.Invoke(LevelEnum.LevelShadow,PlayableEnum.SpawnPoint);
                        break;
                    case PlayableEnum.PlayerReturnSpawnPoint:
                        PoolSignals.Instance.onSetCurrentLevelToVisible?.Invoke(true);
                        PoolSignals.Instance.onDestroyFightLevel?.Invoke();
                        
                            
                        break;
                    
                }
                
            });
        }



        private void OnCloseUnCutScene(PlayableEnum playableEnum)
        {
            switch (playableEnum)
            {
                case PlayableEnum.EnteredFactory:
                    CoreUISignals.Instance.onPlayMusic?.Invoke(SFXTypes.FactoryWhispers);
                    PlayableSignals.Instance.onSetUpCutScene?.Invoke(playableEnum);
                    break;
                
                case PlayableEnum.SpawnPoint:
                    break;
                
                case PlayableEnum.PlayerReturnSpawnPoint:
                    break;
                
            }
            
            blackwBG.GetComponent<CanvasGroup>().DOFade(0f, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                blackwBG.SetActive(false);
                blackwBG.GetComponent<CanvasGroup>().alpha = 1;
                
            });
        }

       


        
    }
}