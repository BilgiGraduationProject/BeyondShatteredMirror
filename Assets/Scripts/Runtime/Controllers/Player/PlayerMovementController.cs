
using System;
using System.Collections;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Enums.Player;
using Runtime.Keys.Input;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {

        #region Self Variables

        #region Serialized Variables
        [SerializeField] private Rigidbody playerRb;
        [SerializeField] private PlayerAnimationController playerAnimationController;
        #endregion

        #region Private Variables
        private PlayerData _playerData;
        private InputParams _inputParams;
        private Transform _cameraTransform;
        private bool _isReadyToMove;
     
        #endregion

        #endregion
        internal void GetPlayerData(PlayerData playerData) => _playerData = playerData;
        internal void GetInputParams(InputParams inputParams) => _inputParams = inputParams;
        internal void GetCameraTransform(Camera cameraTransform) => _cameraTransform = cameraTransform.transform;
        
        internal void OnPlayerReadyToMove(bool condition)
        {
            _isReadyToMove = condition;
            if (_isReadyToMove) return;
            playerAnimationController.GetPlayerSpeed(0);
            playerRb.velocity = Vector3.zero;
        }
        
        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
           
            if (!_isReadyToMove) return;
            var moveDirection = (_cameraTransform.forward * _inputParams.Vertical + _cameraTransform.right * _inputParams.Horizontal);
            if (moveDirection == Vector3.zero) return;
            var playerRotateDirection = new Vector3(moveDirection.x,0, moveDirection.z);
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(playerRotateDirection),_playerData.RotationSpeed * Time.fixedDeltaTime);
            playerRb.velocity = transform.forward * _playerData.PlayerSpeed;
            playerAnimationController.GetPlayerSpeed(playerRb.velocity.magnitude);
            
        }

        internal void OnPlayerPressedLeftShiftButton(bool condition)
        {
            if (condition)
            {
                _playerData.PlayerSpeed *= _playerData.PlayerSpeedMultiplier;
            }
            else
            {
                _playerData.PlayerSpeed /= _playerData.PlayerSpeedMultiplier;
            }
        }
      

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            var newPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            Gizmos.DrawRay(newPos, 2 * transform.forward);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(newPos, 2 * transform.right);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerRb.transform.position,playerRb.transform.forward * 2);
        }


        internal void OnPlayerPressedSpaceButton()
        {
            transform.DORotateQuaternion(Quaternion.LookRotation(_cameraTransform.forward), 0.2f).SetEase(Ease.Flash).OnComplete(() =>
            {
                playerAnimationController.OnPlayerPressedSpaceButton();
                transform.DOMove(transform.position + transform.forward * _playerData.RollDistance, _playerData.RollTime)
                    .SetEase(Ease.Flash);
            });







        }
    }
     
  
    
    


    
    
    
}