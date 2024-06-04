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

        [Header("Hakan")] 
        [SerializeField] private Transform hakanHousePos;

        [Header("SpawnEnemy")] [SerializeField]
        private Transform spawn;
        
        
        [Header("Camera Position")] 
        [SerializeField] private Transform seizingCameraPos;
        [SerializeField] private Transform mirrorCameraPos;
        [SerializeField] private Transform factoryEntryCameraPos;
        [SerializeField] private Transform secretRoomCameraPos;
        [SerializeField] private Transform layingBedCameraPos;
        [SerializeField] private Transform mansionCameraPos;
        [SerializeField] private Transform hakanCameraPos;
        #endregion

        #region Private Variables
        

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
           
               case PlayableEnum.StandFrontOfMirror:
                   return mirrorCameraPos;
   
               case PlayableEnum.EnteredFactory:
                   return factoryEntryCameraPos;

               case PlayableEnum.SecretWall:
                   return secretRoomCameraPos;

               case PlayableEnum.EnteredHouse:
                   return layingBedCameraPos;
             
               case PlayableEnum.Mansion:
                   return mansionCameraPos;
  
               case PlayableEnum.SpawnPoint:
                   return null;
               case PlayableEnum.HakanPos:
                   return hakanCameraPos;
               default:
                   return null;
               
                
                
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
                case PlayableEnum.SpawnPoint:
                    return spawn;
                case PlayableEnum.HakanPos:
                    return hakanHousePos;
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