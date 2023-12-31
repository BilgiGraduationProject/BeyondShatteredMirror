
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
        private float _currentMoveSpeed;
        private bool _isReadyToMove;
        #endregion

        #endregion
        internal void GetPlayerData(PlayerData playerData) => _playerData = playerData;
        internal void OnUpdateParams(InputParams inputParam) => _inputParams = inputParam;
        internal void GetCameraTransform(Camera cameraTransform) => _cameraTransform = cameraTransform.transform;

        internal void OnIsPlayerReadyToMove(bool isReadyToMove)
        {
            _isReadyToMove = isReadyToMove;
            if (!_isReadyToMove)
            {
              StopPlayer();
            }
        }
        internal void OnPlayerReleaseRunButton() => _playerData.PlayerSpeed = _currentMoveSpeed;
        
        internal void OnPlayerPressedRunButton()
        {
            _currentMoveSpeed = _playerData.PlayerSpeed;
            _playerData.PlayerSpeed += _playerData.PlayerSpeed * 2;
        }
        
        private void FixedUpdate ()
        {
            if (!_isReadyToMove) return;
            var moveDirection =  _inputParams.Vertical * _cameraTransform.forward +  _inputParams.Horizontal * _cameraTransform.right;
            moveDirection.Normalize();
            CharacterMove(moveDirection);
        }

        private void CharacterMove(Vector3 moveDirection)
        {
            if (moveDirection != Vector3.zero)
            {
                var newRotateDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
                transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(newRotateDirection), _playerData.RotationSpeed * Time.deltaTime);
                playerRb.velocity = transform.forward * _playerData.PlayerSpeed;
                playerAnimationController.OnGetPlayerSpeed(playerRb.velocity.magnitude);
            }
        }
        
        private void StopPlayer()
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerAnimationController.OnGetPlayerSpeed(playerRb.velocity.magnitude);
        }
        
        internal void OnPlayerPressedRollButton()
        {
            if (_isReadyToMove)
            {
                PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationState.isRolling,true);
            }
            else
            {
                PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationState.isRolling,true);
                playerRb.AddForce(transform.forward * _playerData.RollForce, ForceMode.Force);
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

      

    }
     
  
    
    


    
    
    
}