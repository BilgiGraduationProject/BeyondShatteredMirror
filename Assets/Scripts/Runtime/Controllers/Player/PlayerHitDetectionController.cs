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
        private GameObject enemyObj;
        #endregion
        
        #endregion

        [SerializeField] private float maxDistance;// Maximum distance for raycasting
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerHandTransform;
        [SerializeField] private Transform playerKillTransform;


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
            bool pickUpDidHitYa = pickUpRay.collider is not null;
            CheckForPickUpRay(pickUpDidHitYa,pickUpRay);
            
            var newRay =  _camera.transform.forward;
            _raycastCommands[0] = new RaycastCommand(_camera.transform.position, newRay, maxDistance,LayerMask.GetMask("Interectable","Openable","Puzzle","SearchingEnemy","Wall"));
            Debug.DrawRay(_camera.transform.position,newRay * maxDistance , Color.cyan);
            _jobHandle = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
        }

        private void CheckForPickUpRay(bool pickUpDidHitYa, RaycastHit pickUpRay)
        {
            if (pickUpDidHitYa)
            {
                if ( _collidedObject is not null &&_collidedObject != pickUpRay.collider.gameObject)
                {
                    ChangeColorOfObject(_collidedObject,false,LayerMask.LayerToName(_collidedObject.layer));
                    _didHit = false;
                }
                
                if (!_didHit)
                {
                    if(Vector3.Distance(playerTransform.position,pickUpRay.collider.transform.position) < 3f)
                    {
                        _collidedObject = pickUpRay.collider.gameObject;
                        ChangeColorOfObject(_collidedObject,true,LayerMask.LayerToName(_collidedObject.layer));
                        _didHit = true;
                    }
                }
                
               

            }
            else
            {
                if (_didHit)
                {
                    ChangeColorOfObject(_collidedObject,false,LayerMask.LayerToName(_collidedObject.layer));
                    _didHit = false;
                }
               
                


            }
        }

        private void ChangeColorOfObject(GameObject collidedObject, bool condition, string layerToName)
        {
            if(_collidedObject is null) return;
            
            if (condition)
            {
                switch (layerToName)
                {
                    case "Interectable":
                        InteractableSignals.Instance.onChangeColorOfInteractableObject?.Invoke(true,collidedObject);
                       
                        break;
                    case "Puzzle":
                        PuzzleSignals.Instance.onChangePuzzleColor?.Invoke(collidedObject,true);
                        break;
                    
                    case "Openable":
                        InteractableSignals.Instance.onChangeColorOfInteractableObject?.Invoke(true,collidedObject);
                        break;
                    
                }
               
            }
            else
            {
                switch (layerToName)
                {
                    case "Interectable":
                        InteractableSignals.Instance.onChangeColorOfInteractableObject?.Invoke(false,collidedObject);
                        _collidedObject = null;
                       
                        break;
                    case "Puzzle":
                        PuzzleSignals.Instance.onChangePuzzleColor?.Invoke(collidedObject,false);
                        _collidedObject = null;
                        break;
                    
                    case "Openable":
                        InteractableSignals.Instance.onChangeColorOfInteractableObject?.Invoke(false,collidedObject);
                        _collidedObject = null;
                        break;
                    
                }
                
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
                case "Openable":
                    InteractableSignals.Instance.onInteractableOpenDoor?.Invoke(_collidedObject);
                    break;
                case "Interectable":
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
                    if(playerHandTransform.childCount < 1) return;
                    PuzzleSignals.Instance.onInteractWithPuzzlePieces?.Invoke(_collidedObject,playerHandTransform.GetChild(0).gameObject);
                    break;
                
                case "SearchingEnemy":
                    if (Vector3.Distance(playerTransform.position, _collidedObject.transform.position) > 2f) return;
                    enemyObj = _collidedObject;
                    Debug.LogWarning("Ready to assassinate");
                    PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.StealthKill);
                    playerTransform.LookAt(enemyObj.transform.position);
                    
                    break;
                
            }
           
            
        }

        public void OnPlayerPressedDropItemButton()
        {
            if (playerHandTransform.childCount < 1) return;
            Debug.LogWarning("Drop the object");
            InteractableSignals.Instance.onDropTheInteractableObject?.Invoke(playerHandTransform.GetChild(0).gameObject,playerHandTransform);
        }


        public void OnPlayerReadyToKillTheEnemy()
        {
            enemyObj.transform.parent = playerKillTransform;
            enemyObj.transform.position = playerKillTransform.position;
        }
    }
}