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

        private Camera _camera;
        private NativeArray<RaycastCommand> _raycastCommands;
        private NativeArray<RaycastHit> _raycastHits;
        private JobHandle _jobHandle;
        private GameObject _collidedObject;

        private bool _didHit;
        
        
        #endregion
        
        #endregion

        [SerializeField] private float maxDistance;// Maximum distance for raycasting
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerHandTransform;


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
        private void FixedUpdate()
        {
            
            
            _jobHandle.Complete();
            RaycastHit pickUpRay = _raycastHits[0];
            bool didHitYa = pickUpRay.collider is not null;
            
            if (didHitYa)
            {
                if (_collidedObject != pickUpRay.collider.gameObject)
                {
                    ChangeColorOfObject(_collidedObject,false);
                    _didHit = false;
                }
                
                if (!_didHit)
                {
                   if(Vector3.Distance(playerTransform.position,pickUpRay.collider.transform.position) < 3f)
                   {
                       _collidedObject = pickUpRay.collider.gameObject;
                          ChangeColorOfObject(_collidedObject,true);
                          _didHit = true;
                   }
                }
                
               

            }
            else
            {
                if (_didHit)
                {
                    ChangeColorOfObject(_collidedObject,false);
                    _didHit = false;
                }
               
                


            }
           
            
            var newRay =  _camera.transform.forward;
            _raycastCommands[0] = new RaycastCommand(_camera.transform.position, newRay, maxDistance,LayerMask.GetMask("Pickable","Door","Openable","Puzzle"));
            Debug.DrawRay(_camera.transform.position,newRay * maxDistance , Color.cyan);
            _jobHandle = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
        }

        private void ChangeColorOfObject(GameObject collidedObject, bool condition)
        {
            if(_collidedObject is null) return;
            if (condition)
            {
                InteractableSignals.Instance.onChangeColorOfInteractableObject?.Invoke(true,collidedObject);
            }
            else
            {
                InteractableSignals.Instance.onChangeColorOfInteractableObject?.Invoke(false,collidedObject);
                _collidedObject = null;
            }
        }


        public void GetCameraTransform(Camera cameraTrans)
        {
            _camera = cameraTrans;
        }


        public void OnPlayerPressedPickUpButton()
        {
            if (_collidedObject is null) return;
            var layerName = LayerMask.LayerToName(_collidedObject.layer);
            switch (layerName)
            {
                case "Door":
                    InteractableSignals.Instance.onInteractableOpenDoor?.Invoke(_collidedObject);
                    break;
                case "Pickable":
                    var child = playerHandTransform.childCount > 0;
                    if (child)
                    {
                        Debug.LogWarning("Drop the object first");
                        InteractableSignals.Instance.onDropandPickUpTheInteractableObject?.Invoke(playerHandTransform.GetChild(0).gameObject,_collidedObject,playerHandTransform);
                    }
                    else
                    {
                        Debug.LogWarning("Pick up the object");
                        InteractableSignals.Instance.onPickUpTheInteractableObject?.Invoke(_collidedObject,playerHandTransform);
                    }
                    break;
                case "Puzzle":
                    InteractableSignals.Instance.onPlayerInteractWithPuzzlePart?.Invoke(_collidedObject,playerHandTransform.GetChild(0).gameObject);
                    break;
                
            }
           
            
        }

        public void OnPlayerPressedDropItemButton()
        {
            if (playerHandTransform.childCount < 1) return;
            Debug.LogWarning("Drop the object");
            InteractableSignals.Instance.onDropTheInteractableObject?.Invoke(playerHandTransform.GetChild(0).gameObject,playerHandTransform);
        }
        
    }
}