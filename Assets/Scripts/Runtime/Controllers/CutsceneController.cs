using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections.Generic;
using Runtime.Signals;
using Runtime.Enums.GameManager;
using DG.Tweening;
using UnityEngine.UI;

namespace Runtime.Controllers
{
    public class CutsceneController : MonoBehaviour
    {
        #region Self Variables

        #region Serializable Variables
        
        [SerializeField] private GameObject blackwBG;
        [SerializeField] private List<GameObject> cutsceneList = new List<GameObject>();
        
        #endregion
        
        #region Private Variables
    
        private int _lastIndex;

        #endregion

        #endregion
        
        private void Awake()
        {
            blackwBG.SetActive(false);
            foreach (var go in cutsceneList) go.SetActive(false);
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
            if (Input.GetKeyDown(KeyCode.N))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(0);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            blackwBG.SetActive(false);
            cutsceneList[0].SetActive(true);
            cutsceneList[0].GetComponent<VideoPlayer>().loopPointReached += OnCutsceneFinished;
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StopPlayer);
        }

        private void OnOpenCutscene(int index)
        {
            _lastIndex = index;
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StopPlayer);
            blackwBG.SetActive(false);
            cutsceneList[index].SetActive(true);
            cutsceneList[_lastIndex].GetComponent<CanvasGroup>().alpha = 0;
            cutsceneList[_lastIndex].GetComponent<CanvasGroup>().DOFade(1f, .75f).SetEase(Ease.OutQuad);
            cutsceneList[index].GetComponent<VideoPlayer>().loopPointReached += OnCutsceneFinished;
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
                blackwBG.GetComponent<CanvasGroup>().DOFade(0f, 3f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    blackwBG.GetComponent<CanvasGroup>().alpha = 1;
                    blackwBG.SetActive(false);
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StartPlayer);
                });
            });
        }
    }
}