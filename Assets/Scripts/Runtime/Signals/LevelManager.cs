using System;
using Runtime.Enums.Playable;
using UnityEngine;

namespace Runtime.Signals
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [Header("House")]
        [SerializeField] private Transform seizingHouse;
        [SerializeField] private Transform mirror;
        [SerializeField] private Transform layingBed;


        [Header("Factory")] 
        [SerializeField] private Transform factoryStart;

        #endregion

        #endregion


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerSignals.Instance.onGetLevelCutScenePosition += OnGetLevelCutScenePosition;
        }

        private Transform OnGetLevelCutScenePosition(PlayableEnum cutscene)
        {
            switch (cutscene)
            {
                case PlayableEnum.BathroomLayingSeize:
                    return seizingHouse;
                case PlayableEnum.StandFrontOfMirror:
                    return mirror;
                case PlayableEnum.EnteredHouse:
                    return layingBed;
                default:
                    return null;
            }
        }

       

        private void UnSubscribeEvents()
        {
            PlayerSignals.Instance.onGetLevelCutScenePosition -= OnGetLevelCutScenePosition;
        }


        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}