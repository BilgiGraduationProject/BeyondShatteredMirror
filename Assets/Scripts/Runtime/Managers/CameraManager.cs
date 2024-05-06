using System;
using Cinemachine;
using Runtime.Enums.Camera;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CinemachineStateDrivenCamera _stateDrivenCamera;
        [SerializeField] private Animator cameraAnimator;
       


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
            

        }

       


        private void OnSetCameraTarget()
        {
            var playerManager = GameObject.FindObjectOfType<PlayerManager>().transform;
            _playerFollow = playerManager.GetChild(playerManager.childCount - 1).transform;
            
            
            _stateDrivenCamera.Follow = _playerFollow;
         
        }   

        [Button]
        private void OnChangeCameraState(CameraStateEnum cameraState)
        {
            switch (cameraState)
            {
                case CameraStateEnum.Play:
                    _stateDrivenCamera.Follow = _playerFollow;
                    break;
                case CameraStateEnum.CutScene:
                    _stateDrivenCamera.Follow = null;
                    break;
                
                
            }
            cameraAnimator.SetTrigger(cameraState.ToString());
        }

        private void UnSubscribeEvents()
        {
            CameraSignals.Instance.onChangeCameraState -= OnChangeCameraState;
            CameraSignals.Instance.onSetCinemachineTarget -= OnSetCameraTarget;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        

    }
}