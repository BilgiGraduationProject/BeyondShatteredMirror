using System;
using System.Linq;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
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
            OnSetUpCutScene(PlayableEnum.LayingSeize);
           
        }

        private void Init()
        {
           _playerPlayable = GetPlayable();

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


        

        private void OnSetUpCutScene(PlayableEnum playableEnum)
        {
            PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(playableEnum);
            var assets = _playerPlayable.playerPlayableAssets[(int)playableEnum];
            if (assets is null) return;
            var playableBindings = assets.outputs.ToArray();

            foreach (var binding in playableBindings)
            {
                playableDirector.playableAsset = assets;
                playableDirector.SetGenericBinding(playableDirector, binding.sourceObject);
                playableDirector.Play();
                CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
               
            }
        }


        private void UnSubscribeEvents()
        {
            PlayableSignals.Instance.onSetUpCutScene -= OnSetUpCutScene;
          
        }



    }
}