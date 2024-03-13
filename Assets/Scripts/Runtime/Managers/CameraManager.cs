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

        private Transform _playerTransform;
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
            var playerLookAt = playerManager.GetChild(playerManager.childCount - 1).transform;
            _stateDrivenCamera.Follow = playerLookAt;
            _stateDrivenCamera.LookAt = playerLookAt;
        }   

        private void OnChangeCameraState(CameraStateEnum cameraState)
        {
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