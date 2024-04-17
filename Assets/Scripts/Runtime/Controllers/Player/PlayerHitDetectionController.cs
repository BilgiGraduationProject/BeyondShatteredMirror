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
        private GameObject _obj;
        private bool _isHit;
        private bool _isSearching;
        private Renderer _pickUpRenderer;
        private bool _isInteract;
        


        #endregion


        #region Serialized Variables

        [SerializeField] private Transform _playerHand;
        

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
                if (pickUpRay.collider.CompareTag("Pick") && !_isInteract)
                {
                    if (!_isHit)
                    {
                        _obj = pickUpRay.collider.gameObject;
                        _pickUpRenderer = pickUpRay.collider.gameObject.GetComponent<Renderer>();
                        _pickUpRenderer.material.DOFloat(1, "_OutlineWidth", 1f);
                        _isHit = true;
                        _isSearching = true;
                    }
                    
                }
                
            }
            else
            {
                if (_isSearching && !_isInteract)
                {
                    _pickUpRenderer.material.DOFloat(0, "_OutlineWidth", 1f);
                    _isHit = false;
                    _isSearching = false;
                }
                
            }
            
            
            // 2. Schedule new raycast
            var newRay =  camera.transform.forward;
            _raycastCommands[0] = new RaycastCommand(camera.transform.position, newRay, maxDistance,LayerMask.GetMask("Pick"));
            Debug.DrawRay(camera.transform.position,newRay * maxDistance , Color.cyan);
            _jobHandle = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
        }
        
        

        public void GetCameraTransform(Camera cameraTrans)
        {
            camera = cameraTrans;
        }


        public void OnPlayerInteractWithObject()
        {
            if (_obj is null || _isInteract) return;
            if (Vector3.Distance((Vector3)PlayerSignals.Instance.onGetPlayerTransform?.Invoke().position,
                    _obj.transform.position) < 1f)
            {
                Debug.LogWarning("Can Take the item");
                PlayerSignals.Instance.onGetPlayerTransform?.Invoke().DOLookAt(_obj.transform.position, 1f).OnComplete(
                    () =>
                    {
                        PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.PickUp);
                    });

            }
            else
            {
                Debug.LogWarning("Can't take the item");
                
            }
        }

        public void OnCanPlayerInteractWithSomething(bool condition)
        {
            _isInteract = condition;
        }

        public void OnGetInteractableObject()
        {
            if(_obj is null) return;
            if (_playerHand.childCount > 0)
            {
                var pickUpItem = _playerHand.GetChild(0).gameObject;
                pickUpItem.transform.parent = null;
                pickUpItem.GetComponent<Rigidbody>().useGravity = true;
            }
            _obj.GetComponent<Rigidbody>().useGravity = false;
            _obj.transform.parent = _playerHand;
            _obj.transform.localPosition = Vector3.zero;
            _obj = null;
        }
    }
}