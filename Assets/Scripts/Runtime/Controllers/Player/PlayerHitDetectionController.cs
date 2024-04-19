using System;
using DG.Tweening;
using Runtime.Enums.Player;
using Runtime.Signals;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerHitDetectionController : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private Camera camera;
        private NativeArray<RaycastCommand> _raycastCommands;
        private NativeArray<RaycastHit> _raycastHits;
        private JobHandle _jobHandle;
        private bool _isReadyToInteract;
        private bool _isRaySearching;
        private GameObject _collidedObject;
        
        #endregion
        
        #endregion

        [SerializeField] private float maxDistance;// Maximum distance for raycasting


        private void Awake()
        {
            _raycastCommands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
            _raycastHits = new NativeArray<RaycastHit>(1, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            _jobHandle.Complete();
            _raycastCommands.Dispose();
            _raycastHits.Dispose();
        }
        
       
        [Obsolete("Obsolete")]
        private void Update()
        {
            
            
            _jobHandle.Complete();
            RaycastHit pickUpRay = _raycastHits[0];
            bool didHitYa = pickUpRay.collider is not null;
            
            if (didHitYa)
            {
                
                if (!_isRaySearching)
                {
                    Debug.LogWarning("Ray colliding with something");
                    _collidedObject = pickUpRay.collider.gameObject;
                    ChangeColorOfCollectable(_collidedObject,true);
                    _isReadyToInteract = true;
                    _isRaySearching = true;
                }
                

            }
            else
            {
                if (_isReadyToInteract && _isRaySearching)
                {
                    Debug.LogWarning("Ray stop colliding with something");
                    ChangeColorOfCollectable(_collidedObject,false);
                    _collidedObject = null;
                    _isReadyToInteract = false;
                    _isRaySearching = false;

                }
               
                
            }
           
            
            var newRay =  camera.transform.forward;
            _raycastCommands[0] = new RaycastCommand(camera.transform.position, newRay, maxDistance,LayerMask.GetMask("Pick","Door"));
            Debug.DrawRay(camera.transform.position,newRay * maxDistance , Color.cyan);
            _jobHandle = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
        }

        private void ChangeColorOfCollectable(GameObject collidedObject, bool condition)
        {
                CollectableSignals.Instance.onChangeCollectableColor?.Invoke(collidedObject,condition);
        }


        public void GetCameraTransform(Camera cameraTrans)
        {
            camera = cameraTrans;
        }


        public void OnPlayerPressedPickUpButton()
        {
            if (_collidedObject is null) return;
            Debug.LogWarning("Item is not null");
            PlayerSignals.Instance.onPlayerStartToPickUp?.Invoke(_collidedObject);
        }
    }
}