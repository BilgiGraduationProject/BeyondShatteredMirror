using Runtime.Data.ValueObject;
using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerGunController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        

        #endregion

        #region Private Variables

        private PlayerData _playerData;
        private bool _isPistolTakeOut;
        private bool _canShoot;

        #endregion

        #endregion
        public void GetPlayerData(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void OnIsPlayerReadyToShoot(bool condition)
        {
            Debug.LogWarning(condition);
            if (condition == _isPistolTakeOut) return;
            _isPistolTakeOut = condition;
            if (condition)
            {
                PlayerSignals.Instance.onTriggerPlayerAnimationState?.Invoke(PlayerAnimationState.PistolTakeOut
                    .ToString());
            }
            else
            {
                PlayerSignals.Instance.onTriggerPlayerAnimationState?.Invoke(PlayerAnimationState.PistolTakeIn
                    .ToString());
            }
        }

        public void OnPlayerPressedAimButton()
        {
            Debug.LogWarning("Aiming");
            if (!_isPistolTakeOut) return;
            PlayerSignals.Instance.onChangeAnimationLayerWeight?.Invoke((int)PlayerAnimationLayerEnum.PistolAim,1,1);
            
        }

        public void OnPlayerReleasedMouseButtonRight()
        {
            if(!_isPistolTakeOut) return;
            PlayerSignals.Instance.onChangeAnimationLayerWeight?.Invoke((int)PlayerAnimationLayerEnum.PistolAim,0,1);
        }

        public void OnPlayerPressedShootButton()
        {
            if (_canShoot)
            {
                PlayerSignals.Instance.onTriggerPlayerAnimationState?.Invoke(PlayerAnimationState.isShooting.ToString());
            }
            else
            {
                Debug.LogWarning("Can't Shoot");
            }
        }

        public void OnPlayerCanShoot(bool condition) => _canShoot = condition;
        
    }
}