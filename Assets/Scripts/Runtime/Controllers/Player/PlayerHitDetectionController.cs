﻿using System;
using DG.Tweening;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Enums.Puzzle;
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


        private bool _isCanControl = false;

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
            _raycastCommands[0] = new RaycastCommand(_camera.transform.position, newRay, maxDistance,LayerMask.GetMask("Interectable","Openable","Puzzle","SearchingEnemy","Wall","DetectiveBoard","MemoryCard","MansionTel"));
            Debug.DrawRay(_camera.transform.position,newRay * maxDistance , Color.cyan);
            _jobHandle = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
        }

        private void CheckForPickUpRay(bool pickUpDidHitYa, RaycastHit pickUpRay)
        {
            

            if (_isCanControl)
            {
                if (pickUpDidHitYa)
                {
                    if ( _collidedObject is not null && _collidedObject != pickUpRay.collider.gameObject)
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

                    if (_collidedObject is null)
                    {
                        _didHit = false;
                    }
                
               

                }
                else
                {
                    if (_didHit )
                    {
                   
                        ChangeColorOfObject(_collidedObject,false,LayerMask.LayerToName(_collidedObject.layer));
                        _didHit = false;
                    }
               
                


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
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(true,collidedObject);
                       
                        break;
                    case "Puzzle":
                    if(_collidedObject is null) return;
                        PuzzleSignals.Instance.onChangePuzzleColor?.Invoke(collidedObject,true);
                        break;
                    
                    case "Openable":
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(true,collidedObject);
                        break;
                    case "DetectiveBoard":
                    if(_collidedObject is null) return;
                        _collidedObject.GetComponent<MeshRenderer>().material.DOFloat(1.8f, "_OutlineWidth", 1);
                        break;
                    case "MemoryCard":
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(true,collidedObject);
                        break;
                    case "MansionTel":
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(true,collidedObject);
                        break;
                    default:
                        break;
                }
               
            }
            else
            {
                switch (layerToName)
                {
                    case "Interectable":
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(false,collidedObject);
                        _collidedObject = null;
                       
                        break;
                    case "Puzzle":
                    if(_collidedObject is null) return;
                        PuzzleSignals.Instance.onChangePuzzleColor?.Invoke(collidedObject,false);
                        _collidedObject = null;
                        break;
                    
                    case "Openable":
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(false,collidedObject);
                        _collidedObject = null;
                        break;
                    case "DetectiveBoard":
                    if(_collidedObject is null) return;
                        collidedObject.GetComponent<MeshRenderer>().material.DOFloat(0, "_OutlineWidth", 1);
                        break;
                    case "MemoryCard":
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(false,collidedObject);
                        _collidedObject = null;
                        break;
                    case "MansionTel":
                    if(_collidedObject is null) return;
                        ChangeColorOfInteractableObject(false,collidedObject);
                        _collidedObject = null;
                        break;
                    default:
                        break;
                    
                }
                
            }
        }

        private void ChangeColorOfInteractableObject(bool condition, GameObject collidedObject)
        {
            // if (condition)
            // {
            //     collidedObject.GetComponent<MeshRenderer>().material.DOFloat(1, "_OutlineWidth", 1);
            // }
            // else
            // {
            //     collidedObject.GetComponent<MeshRenderer>().material.DOFloat(0, "_OutlineWidth", 1);
            // }
            if (collidedObject is null) return;
            if(_collidedObject is null) return;
            
            var meshRenderer = _collidedObject.GetComponent<MeshRenderer>();

            var childMeshRenderers = _collidedObject.GetComponentsInChildren<MeshRenderer>();
            
            if (meshRenderer)
            {
                meshRenderer.material.DOFloat(condition ? 1 : 0, "_OutlineWidth", 1);
            }
            else
            {
                foreach (var childMeshRenderer in childMeshRenderers)
                {
                    childMeshRenderer.material.DOFloat(condition ? 1 : 0, "_OutlineWidth", 1);
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
                    Openable();
                    break;
                case "Interectable":
                    if(_collidedObject.TryGetComponent(out PillCollectable pillCollectable))
                    {
                        pillCollectable.CollectPill();
                        return;
                    }
                    
                    var child = playerHandTransform.childCount > 0;
                    if (child)
                    {
                        Debug.LogWarning("Drop the object first");
                        DropTheObjectFirst();
                    }
                    else
                    {
                        Debug.LogWarning("Pick up the object");
                        PickUpTheObject();
                    }
                    break;
                case "Puzzle":
                    if(playerHandTransform.childCount < 1) return;
                    PuzzleSignals.Instance.onInteractWithPuzzlePieces?.Invoke(_collidedObject,playerHandTransform.GetChild(0).gameObject);
                    break;
                
                case "SearchingEnemy":
                    
                    break;
                
                case "DetectiveBoard":
                    CoreUISignals.Instance.onOpenCutscene?.Invoke(3);
                    _collidedObject.layer = 0;
                    break;
                
                case "MemoryCard":
                    InputSignals.Instance.onSetPickUpButton?.Invoke(true);
                    CoreUISignals.Instance.onOpenUnCutScene?.Invoke(PlayableEnum.SpawnPoint);
                    break;
                case "MansionTel":
                    CoreUISignals.Instance.onOpenCutscene?.Invoke(4);
                    break;
            }
        }

        private void Openable()
        {
            var openableAnim = _collidedObject.GetComponent<Animator>();
            openableAnim.SetBool("Open",!openableAnim.GetBool("Open"));
        }

        private void PickUpTheObject()
        {
            if (PuzzleSignals.Instance.onGetPuzzleEnum?.Invoke() == PuzzleEnum.Lantern)
            {
                var objRb = _collidedObject.GetComponent<Rigidbody>();
                objRb.useGravity = false;
                objRb.isKinematic = true;
                _collidedObject.transform.parent = playerHandTransform;
                var newPos = new Vector3(0.08f, 0.17f, 0.38f);
                    
                _collidedObject.transform.localRotation = Quaternion.Euler(28,-180,9);
                _collidedObject.transform.localPosition = newPos;
            }
            else
            {
                var objRb = _collidedObject.GetComponent<Rigidbody>();
                objRb.useGravity = false;
                objRb.isKinematic = true;
                _collidedObject.transform.parent = playerHandTransform;
                _collidedObject.transform.localRotation = Quaternion.identity;
                _collidedObject.transform.localPosition = playerHandTransform.localPosition;
            }
            
        }

        private void DropTheObjectFirst()
        {
            var playerHandObj = playerHandTransform.GetChild(0).gameObject;
            playerHandObj.transform.parent = null;
            var rb = playerHandObj.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            var newObj = _collidedObject.GetComponent<Rigidbody>();
            newObj.useGravity = false;
            newObj.isKinematic = true;
            _collidedObject.transform.parent = playerHandTransform;
            _collidedObject.transform.localPosition = Vector3.zero;
        }

        public void OnPlayerPressedDropItemButton()
        {
            if (playerHandTransform.childCount < 1) return;
            Debug.LogWarning("Drop the object");
            var dropObj = playerHandTransform.GetChild(0).gameObject;
            var rb  =dropObj.GetComponent<Rigidbody>();
            dropObj.transform.parent = null;
            rb.useGravity = true;
            rb.isKinematic = false;
        }


        public void OnPlayerReadyToKillTheEnemy()
        {
            enemyObj.transform.parent = playerKillTransform;
            enemyObj.transform.position = playerKillTransform.position;
        }


        public void EmptyThePlayerHand()
        {
            OnPlayerPressedDropItemButton();
        }

        public void OnCanPlayerCheckItems(bool condition)
        {
            _isCanControl = condition;
        }

        public void OnSetCollidedObjectNull()
        {
            _collidedObject = null;
        }
    }
}