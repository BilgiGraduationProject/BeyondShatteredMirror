using System;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Enums.GameManager;
using Runtime.Enums.Player;
using Runtime.Keys.Input;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Controllers.Player
{
    public class PlayerEnemyDetectionController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        #endregion

        #region Private Variables

        private Camera _camera;
        private PlayerData _playerData;
        private InputParams _inputParams;

        private GameObject _enemyTransform;
        private bool _isEnemyAlive;
        
        #endregion

        #endregion
        internal void GetPlayerData(PlayerData playerData) => _playerData = playerData;
        internal void GetCameraTransform(Camera cameraTransform) => _camera = cameraTransform;
        internal void OnUpdateParams(InputParams inputParams) => _inputParams = inputParams;

        private void FixedUpdate()
        {
            transform.forward = _camera.transform.forward;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _enemyTransform = other.gameObject.transform.parent.gameObject;
                EnemySignals.Instance.onShowEnemyHealthBar?.Invoke(_enemyTransform);
                Debug.LogWarning("Enemy is not null");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemySignals.Instance.onHideEnemyHealthBar?.Invoke(other.gameObject.transform.parent.gameObject);
                _enemyTransform = null;
                Debug.LogWarning("Enemy is null");
                
            }
        }

        internal GameObject GetEnemyTransform()
        {
            Debug.LogWarning(_enemyTransform);
            return _enemyTransform;
        } 


    }
}