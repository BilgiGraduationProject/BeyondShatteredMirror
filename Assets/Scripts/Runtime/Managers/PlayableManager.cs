using System;
using System.Collections;
using System.Linq;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.Camera;
using Runtime.Enums.GameManager;
using Runtime.Enums.Playable;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Managers
{
    public class PlayableManager : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;
        private PlayerPlayableData _playerPlayable;


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


        private PlayerPlayableData GetPlayable() => Resources.Load<CD_PlayerPlayable>("Data/CD_PlayerPlayable").PlayerPlayable;
        

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


        

        private void OnSetUpCutScene(PlayableEnum playableEnum,DirectorWrapMode directorMode)
        {
            PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(playableEnum);
            var assets = _playerPlayable.playerPlayableAssets[(int)playableEnum];
            CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.CutScene);
            if (assets is null)
            {
                
            }
            var playableBindings = assets.outputs.ToArray();

            foreach (var binding in playableBindings)
            {
                playableDirector.playableAsset = assets;
                playableDirector.SetGenericBinding(playableDirector, binding.sourceObject);
                playableDirector.Play();
                CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
                playableDirector.extrapolationMode = directorMode;

            }

            if (directorMode is DirectorWrapMode.Hold) return;
            StartCoroutine(OnCutSceneFinished((float)playableDirector.duration));
           
        }

        private IEnumerator OnCutSceneFinished(float playableDirectorDuration)
        {
            yield return new WaitForSeconds(playableDirectorDuration);
            CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.Play);
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
            Debug.LogWarning("How many time is executed");
        }


        private void UnSubscribeEvents()
        {
            PlayableSignals.Instance.onSetUpCutScene -= OnSetUpCutScene;
          
        }



    }
}