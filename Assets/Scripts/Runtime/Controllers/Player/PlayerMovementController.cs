
using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Runtime.Data.ValueObject;
using Runtime.Enums.Player;
using Runtime.Keys.Input;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {

        #region Self Variables

        #region Serialized Variables
        [SerializeField] private Rigidbody playerRb;
        [SerializeField] private PlayerAnimationController playerAnimationController;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float gravityMultiplier = 3.0f;
        [SerializeField] private float _velocity;
        
        #endregion

        #region Private Variables
        private PlayerData _playerData;
        private InputParams _inputParams;
        private Transform _cameraTransform;
        private bool _isReadyToMove;
        private TweenerCore<Vector3,Vector3,VectorOptions> _dotWeenRoll;
        private bool _isFalling;
        private bool _isKillRoll;
        private bool _isRolling;
       
        #endregion

        #endregion
        internal void GetPlayerData(PlayerData playerData) => _playerData = playerData;
        internal void GetInputParams(InputParams inputParams) => _inputParams = inputParams;
        internal void GetCameraTransform(Camera cameraTransform) => _cameraTransform = cameraTransform.transform;
        
        internal void OnPlayerReadyToMove(bool condition)
        {
            _isReadyToMove = condition;
            if (_isReadyToMove) return;
            Debug.LogWarning("Playyer is not moving");
            PlayerSignals.Instance.onGetPlayerSpeed?.Invoke(0);
            playerRb.velocity = Vector3.zero;
           
        }
        
        private void FixedUpdate()
        {
            MovePlayer();
            
            

        }

        private void MovePlayer()
        {
            if (!_isReadyToMove || _isRolling) return;
            var moveDirection = (_cameraTransform.forward * _inputParams.Vertical + _cameraTransform.right * _inputParams.Horizontal);
            if (moveDirection == Vector3.zero) return;
            var playerRotateDirection = new Vector3(moveDirection.x,0, moveDirection.z);
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(playerRotateDirection),_playerData.RotationSpeed * Time.fixedDeltaTime);
            var newMoveDirection = new Vector3(transform.forward.x,0,transform.forward.z);
            playerRb.velocity = newMoveDirection * _playerData.PlayerSpeed;
            PlayerSignals.Instance.onGetPlayerSpeed?.Invoke(playerRb.velocity.magnitude);
            
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
            if (_isRolling) return;
            var newLookDirection = Quaternion.Euler(0,_cameraTransform.eulerAngles.y,0);
            transform.DORotateQuaternion(newLookDirection, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
            {
                playerRb.AddForce((transform.forward * _playerData.RollForce) / _playerData.PlayerSpeed,ForceMode.Impulse);
                PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Roll,true);
                
            });
          

        }


        public void OnPlayerIsRolling(bool condition)
        {
            _isRolling = condition;
        }
    }
     
  
    
    


    
    
    
}