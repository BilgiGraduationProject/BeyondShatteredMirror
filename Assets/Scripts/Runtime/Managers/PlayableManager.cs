using System;
using System.Collections;
using System.Linq;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.Camera;
using Runtime.Enums.GameManager;
using Runtime.Enums.Playable;
using Runtime.Enums.Pool;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Managers
{
    public class PlayableManager : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;
        private CD_PlayerPlayable _playerPlayable;
        [SerializeField] private GameObject aslanHouse;
        [SerializeField] private GameObject factory;


        private void Awake()
        {
            Init();
            
           
        }

        private void Init()
        {
            _playerPlayable = GetPlayable();

        }

        private void Start()
        {
            //OnSetUpCutScene(PlayableEnum.LayingSeize,DirectorWrapMode.None);
        }


        private CD_PlayerPlayable GetPlayable() => Resources.Load<CD_PlayerPlayable>("Data/CD_PlayerPlayable");
        

        public void OnEnable()
        {
            SubscribeEvents();
        }


        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayableSignals.Instance.onSetUpCutScene += OnSetUpCutScene;
            
        }


        

        private void OnSetUpCutScene(PlayableEnum playableEnum)
        {
           
            var assets = _playerPlayable.PlayerPlayable[(int)playableEnum].playerPlayableAssets;
            var directorMode = _playerPlayable.PlayerPlayable[(int)playableEnum].directorWrapMode;
            
            if (assets is null) return;
            
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
            var playableBindings = assets.outputs.ToArray();
            
            foreach (var binding in playableBindings)
            {
                playableDirector.playableAsset = assets;
                playableDirector.SetGenericBinding(playableDirector, binding.sourceObject);
                playableDirector.Play();
                 playableDirector.extrapolationMode = directorMode;

            }

            StartCoroutine(directorMode is DirectorWrapMode.Hold
                ? OnHoldCutScene((float)playableDirector.duration,playableEnum)
                : OnCutSceneFinished((float)playableDirector.duration));
        }

        private IEnumerator OnHoldCutScene(float playableDirectorDuration,PlayableEnum playableEnum)
        {
            yield return new WaitForSeconds(playableDirectorDuration);
            switch (playableEnum)
            {
                case PlayableEnum.BathroomLayingSeize:
                    factory.SetActive(false);
                    aslanHouse.SetActive(true);
                    PlayableSignals.Instance.onSendInputManagerToReadyForInput?.Invoke(true,playableEnum);
                    break;
                case PlayableEnum.StandFrontOfMirror:
                    aslanHouse.SetActive(false);
                    factory.SetActive(true);
                    PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.EnteredFactory);
                    break;
                case PlayableEnum.EnteredHouse:
                    factory.SetActive(false);
                    aslanHouse.SetActive(true);
                    PlayableSignals.Instance.onSendInputManagerToReadyForInput?.Invoke(true,playableEnum);
                    break;
                
            }
            
            
        }

        private IEnumerator OnCutSceneFinished(float playableDirectorDuration)
        {
            yield return new WaitForSeconds(playableDirectorDuration);
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
            
        }


        private void UnSubscribeEvents()
        {
            PlayableSignals.Instance.onSetUpCutScene -= OnSetUpCutScene;
          
        }



    }
}