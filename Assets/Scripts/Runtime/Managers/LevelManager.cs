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


        [Header("Mansion")] 
        [SerializeField] private Transform mansion;


        [Header("Camera Position")] 
        [SerializeField] private Transform seizingCameraPos;
        [SerializeField] private Transform mirrorCameraPos;
        [SerializeField] private Transform factoryEntryCameraPos;
        [SerializeField] private Transform secretRoomCameraPos;
        [SerializeField] private Transform layingBedCameraPos;
        [SerializeField] private Transform mansionCameraPos;
        #endregion

        #endregion


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerSignals.Instance.onGetLevelCutScenePosition += OnGetLevelCutScenePosition;
           CoreGameSignals.Instance.onGetCameraCutScenePosition += OnGetCameraCutScenePosition;
        }

        private Transform OnGetCameraCutScenePosition(PlayableEnum type)
        {
            Debug.LogWarning(type);
            switch (type)
            {
                
               case PlayableEnum.BathroomLayingSeize:
                   return seizingCameraPos;
                   break;
               case PlayableEnum.StandFrontOfMirror:
                   return mirrorCameraPos;
                   break;
               case PlayableEnum.EnteredFactory:
                   return factoryEntryCameraPos;
                   break;
               case PlayableEnum.SecretWall:
                   return secretRoomCameraPos;
                   break;
               case PlayableEnum.EnteredHouse:
                   return layingBedCameraPos;
                   break;
               case PlayableEnum.Mansion:
                   return mansionCameraPos;
                   break;
               default:
                   return seizingCameraPos;
               
                
                
            }
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
                case PlayableEnum.EnteredFactory:
                    return factoryStart;
                case PlayableEnum.Mansion:
                    return mansion;
                
                default:
                    return null;
            }
        }

       

        private void UnSubscribeEvents()
        {
            PlayerSignals.Instance.onGetLevelCutScenePosition -= OnGetLevelCutScenePosition;
            CoreGameSignals.Instance.onGetCameraCutScenePosition -= OnGetCameraCutScenePosition;
        }


        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}