﻿using System;
using System.Collections;
using System.Linq;
using Runtime.Controllers.Player;
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
using UnityEngine.Timeline;

namespace Runtime.Managers
{
    public class PlayableManager : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;
        private CD_PlayerPlayable _playerPlayable;
        private bool _isPlayableStarted;


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
            CoreUISignals.Instance.onDisableAllPanels?.Invoke();
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
            var playableBindings = assets.outputs.ToArray();
            playableDirector.playableAsset = assets;

            playableDirector.extrapolationMode = directorMode;
            foreach (var binding in playableBindings)
            {
                var obj = playableDirector.GetGenericBinding(binding.sourceObject);
                if (obj is null)
                {
                    var timelineAsset = playableDirector.playableAsset as TimelineAsset;
                    var track = timelineAsset.GetOutputTracks().FirstOrDefault(t=>t.name == binding.streamName);
                    switch (track.name)
                    {
                        case "MirrorAnimTrack":
                            var getMirrorAnim = GameObject.FindWithTag("MirrorAnim").GetComponent<Animator>();
                            playableDirector.SetGenericBinding(track, getMirrorAnim);
                            break;
                        case "SecretWall":
                            var getWallAnim = GameObject.FindWithTag("SecretWall").GetComponent<Animator>();
                            playableDirector.SetGenericBinding(track, getWallAnim);
                            break;
                        case "Player":
                            var getPlayer = FindObjectOfType<PlayerAnimationController>().gameObject
                                .GetComponent<Animator>();
                            playableDirector.SetGenericBinding(track, getPlayer);
                            break;
                        case "Cutscene":
                            var cutscene = GameObject.FindWithTag("Cutscene").GetComponent<Animator>();
                            playableDirector.SetGenericBinding(track, cutscene);
                            break;
                        case "Hakan":
                            var hakan = GameObject.FindWithTag("Hakan").GetComponent<Animator>();
                            playableDirector.SetGenericBinding(track, hakan);
                            break;
                        case "Sound":
                            var sound = Camera.main.GetComponent<AudioSource>();
                            playableDirector.SetGenericBinding(track, sound);
                            break;

                    }
                }
                else
                {
                    playableDirector.SetGenericBinding(playableDirector, binding.sourceObject);

                }

               
            }
            playableDirector.Play();

           
            StartCoroutine(directorMode is DirectorWrapMode.Hold
                ? OnHoldCutScene((float)playableDirector.duration, playableEnum)
                : OnCutSceneFinished((float)playableDirector.duration,playableEnum));
        }

        

        private IEnumerator OnHoldCutScene(float playableDirectorDuration, PlayableEnum playableEnum)
            {
                yield return new WaitForSeconds(playableDirectorDuration);
                switch (playableEnum)
                {
                    case PlayableEnum.BathroomLayingSeize:
                        PlayableSignals.Instance.onSendInputManagerToReadyForInput?.Invoke(true, playableEnum);
                        break;
                    case PlayableEnum.StandFrontOfMirror:
                        CoreUISignals.Instance.onOpenUnCutScene?.Invoke(playableEnum);
                        CoreUISignals.Instance.onStopMusic?.Invoke();
                        break;
                    case PlayableEnum.EnteredHouse:
                      
                        PlayableSignals.Instance.onSendInputManagerToReadyForInput?.Invoke(true, playableEnum);
                        break;
                }

               


            }

            private IEnumerator OnCutSceneFinished(float playableDirectorDuration, PlayableEnum playableEnum)
            {
                yield return new WaitForSeconds(playableDirectorDuration);
                CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
                CoreUISignals.Instance.onEnableAllPanels?.Invoke();
                switch (playableEnum)
                {
                    case PlayableEnum.ShowHakan:
                        InputSignals.Instance.onIsReadyForCombat?.Invoke(true);
                        EnemySignals.Instance.onStartHakanRun?.Invoke();
                        EnemySignals.Instance.onOpenHakanUI?.Invoke();
                        break;
                    
                }

            }


            private void UnSubscribeEvents()
            {
                PlayableSignals.Instance.onSetUpCutScene -= OnSetUpCutScene;

            }



        }
    }
