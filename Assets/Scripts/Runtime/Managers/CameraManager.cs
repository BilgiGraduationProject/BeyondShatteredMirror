using System;
using Cinemachine;
using Runtime.Enums.Camera;
using Runtime.Enums.Playable;
using Runtime.Signals;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CinemachineStateDrivenCamera _stateDrivenCamera;
        [SerializeField] private Animator cameraAnimator;
        [SerializeField] private Transform cutsSceneCamera;
       


        #endregion

        #region Private Variables
        
        private Transform _playerFollow;
        private Vector3 _viewDirection;

        #endregion

     

        #endregion
        

        private void Awake()
        {
            OnSetCameraTarget();
            
        }
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CameraSignals.Instance.onChangeCameraState += OnChangeCameraState;
            CameraSignals.Instance.onSetCinemachineTarget += OnSetCameraTarget;
            CameraSignals.Instance.onSetCameraPositionForCutScene += OnSetCameraPositionForCutScene;

        }

        private void OnSetCameraPositionForCutScene(PlayableEnum playable)
        {
            Debug.LogWarning(playable);
            
            switch (playable)
            {
                case PlayableEnum.BathroomLayingSeize:
                    var seizingPos = CoreGameSignals.Instance.onGetCameraCutScenePosition?.Invoke(playable);
                    cutsSceneCamera.transform.position = new Vector3(0, 0, 0);
                    if(seizingPos is null) return;
                    _stateDrivenCamera.transform.position = seizingPos.position; 
                    _stateDrivenCamera.transform.rotation = seizingPos.rotation;
                    break;
                        
                case PlayableEnum.StandFrontOfMirror:
                    var mirrorPos = CoreGameSignals.Instance.onGetCameraCutScenePosition?.Invoke(playable);
                    cutsSceneCamera.transform.position = new Vector3(0, 0, 0);
                    if(mirrorPos is null) return;
                    _stateDrivenCamera.transform.position = mirrorPos.position; 
                    _stateDrivenCamera.transform.rotation = mirrorPos.rotation;
                    break;
                case PlayableEnum.EnteredFactory:
                    var factoryEntry = CoreGameSignals.Instance.onGetCameraCutScenePosition?.Invoke(playable);
                    cutsSceneCamera.transform.position = new Vector3(0, 0, 0);
                    if(factoryEntry is null) return;
                    _stateDrivenCamera.transform.position = factoryEntry.position; 
                    _stateDrivenCamera.transform.rotation = factoryEntry.rotation;
                    break;
                case PlayableEnum.SecretWall:
                    var secretWall = CoreGameSignals.Instance.onGetCameraCutScenePosition?.Invoke(playable);
                    cutsSceneCamera.transform.position = new Vector3(0, 0, 0);
                    if(secretWall is null) return;
                    _stateDrivenCamera.transform.position = secretWall.position; 
                    _stateDrivenCamera.transform.rotation = secretWall.rotation;
                    break;
                            
            }
        

        }
        


        private void OnSetCameraTarget()
        {
            var playerManager = GameObject.FindObjectOfType<PlayerManager>().transform;
            _playerFollow = playerManager.GetChild(playerManager.childCount - 1).transform;
            
            
            _stateDrivenCamera.Follow = _playerFollow;
         
        }   
        
        private void OnChangeCameraState(CameraStateEnum cameraState)
        {
            cameraAnimator.SetTrigger(cameraState.ToString());
            switch (cameraState)
            {
                case CameraStateEnum.Play:
                    _stateDrivenCamera.Follow = _playerFollow;
                    break;
                case CameraStateEnum.CutScene:
                    _stateDrivenCamera.Follow = null;
                    break;
                    
                  
                
            }
            
        }

        private void UnSubscribeEvents()
        {
            CameraSignals.Instance.onChangeCameraState += OnChangeCameraState;
            CameraSignals.Instance.onSetCinemachineTarget += OnSetCameraTarget;
            CameraSignals.Instance.onSetCameraPositionForCutScene += OnSetCameraPositionForCutScene;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        

    }
}