using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.Collectable;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Enums.Pool;
using Runtime.Signals;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Runtime.Managers
{
    public class CollectableManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CollectableEnum _collectableType;
        [SerializeField] private Animator animator;
        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private GameObject catRightEye;
        [SerializeField] private GameObject catLeftEye;
        [SerializeField] private RawImage _viewFinderImage;
        #endregion

        #region Private Variables

        private CD_Collectable _collectableData;
        private readonly string _pathOfData = "Data/CD_Collectable";
        
        #endregion

        #endregion

        private void Awake()
        {
            _collectableData = GetCollectableData();
         
        }

        
        private CD_Collectable GetCollectableData() => Resources.Load<CD_Collectable>(_pathOfData);

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CollectableSignals.Instance.onCollectableDoJob += OnCollectableDoJob;
            CollectableSignals.Instance.onCheckCollectableType += OnCheckCollectableType;
            CollectableSignals.Instance.onChangeCollectableColor += OnChangeCollectableColor;
        }

        private void OnChangeCollectableColor(GameObject colObj, bool condition)
        {
            if(colObj.GetInstanceID() != gameObject.GetInstanceID()) return;
            if (condition)
            {
                if (_collectableType == CollectableEnum.Puzzle)
                {
                    Debug.LogWarning("Do something about puzzle");
                   
                }
                else if (_collectableType == CollectableEnum.Photo)
                {
                    _viewFinderImage.DOColor(Color.green, 1);
                }
                else
                {
                    renderer.material.DOFloat(1, "_OutlineWidth", 1);
                }
                    
                
              
            }
           
            else
            {
                if (_collectableType == CollectableEnum.Puzzle)
                {
                    Debug.LogWarning("Do something");
                }
                else if (_collectableType == CollectableEnum.Photo)
                {
                    _viewFinderImage.DOColor(Color.white, 1);
                }
                else
                {
                    renderer.material.DOFloat(0, "_OutlineWidth", 1);

                }
               
                
            }
        }

        private void OnCheckCollectableType(GameObject collectableObj)
        {
            if(collectableObj.GetInstanceID() != gameObject.GetInstanceID()) return;
            CollectableSignals.Instance.onSendCollectableType?.Invoke(_collectableType);
        }

        private void OnCollectableDoJob(GameObject collectableObj,CollectableEnum collectableType)
        {
            Debug.LogWarning(collectableObj.name + collectableType.ToString());
            if(collectableObj.GetInstanceID() != gameObject.GetInstanceID()) return;

            switch (collectableType)
            {
                case CollectableEnum.Open:
                   var isOpen = animator.GetBool("Open");
                     animator.SetBool("Open",!isOpen);
                   Debug.LogWarning("Opened door");
                    break;
                
                case CollectableEnum.Carryable:
                    rigidbody.useGravity = false;
                    break;
                
                case CollectableEnum.Puzzle:
                    var tagOfItem = PlayerSignals.Instance.onSendPlayerItemTag?.Invoke();
                    Debug.LogWarning("Player item tag is :" + tagOfItem);
                    switch (tagOfItem.tag)
                    {
                        case "null":
                           
                            break;
                        case "CatLeftEye":
                            catLeftEye.SetActive(true);
                            Destroy(tagOfItem);
                            if (catRightEye.activeSelf)
                            {
                                PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.Puzzle1);
                            }
                            break;
                        case "CatRightEye":
                            Destroy(tagOfItem);
                            catRightEye.SetActive(true);
                            if (catLeftEye.activeSelf)
                            {
                                PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.Puzzle1);
                            }
                           
                            break;
                        
                    }

                    break;
                
            }
            
        }

       


        private void UnSubscribeEvents()
        {
            CollectableSignals.Instance.onCollectableDoJob -= OnCollectableDoJob;
            CollectableSignals.Instance.onCheckCollectableType -= OnCheckCollectableType;
            CollectableSignals.Instance.onChangeCollectableColor -= OnChangeCollectableColor;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }



        #region  Anim Events

        public void AnimEventOnOpenDoor()
        {
            
        }
        

        #endregion
    }
    
    
}