using System;
using System.Numerics;
using DG.Tweening;
using Runtime.Enums.Collectable;
using Runtime.Signals;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Managers
{
    public class InteractableManager : MonoBehaviour
    {
        #region Self Variables
        

        #region Serialized Variables

        [SerializeField] private CollectableEnum type;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Animator animator;

        #endregion

        #endregion


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InteractableSignals.Instance.onChangeColorOfInteractableObject += OnChangeColorOfInteractableObject;
            InteractableSignals.Instance.onDropTheInteractableObject += OnDropTheInteractableObject;
            InteractableSignals.Instance.onPickUpTheInteractableObject += OnPickUpTheInteractableObject;
            InteractableSignals.Instance.onDropandPickUpTheInteractableObject += OnDropandPickUpTheInteractableObject;
            InteractableSignals.Instance.onInteractableOpenDoor += OnInteractableOpenDoor;
          
        }
        private void OnInteractableOpenDoor(GameObject obj)
        {
            if(obj.GetInstanceID() != gameObject.GetInstanceID()) return;
            Debug.LogWarning("Open the door");
            animator.SetBool("Open",!animator.GetBool("Open"));
        }

        private void OnDropandPickUpTheInteractableObject(GameObject playerHandObj, GameObject obj, Transform playerHandTransform)
        {
            if(obj.GetInstanceID() != gameObject.GetInstanceID()) return;
            playerHandObj.transform.parent = null;
            var rb = playerHandObj.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            obj.transform.parent = playerHandTransform;
            obj.transform.localPosition = Vector3.zero;
        }
        private void OnPickUpTheInteractableObject(GameObject obj, Transform playerHandTransform)
        {
            if(obj.GetInstanceID() != gameObject.GetInstanceID()) return;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            obj.transform.parent = playerHandTransform;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localPosition = playerHandTransform.localPosition;
        }

        private void OnDropTheInteractableObject(GameObject obj, Transform playerHandTransform)
        {
            if(obj.GetInstanceID() != gameObject.GetInstanceID()) return;
            obj.transform.parent = null;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }

        private void OnChangeColorOfInteractableObject(bool condition, GameObject obj)
        {
            if(obj.GetInstanceID() != gameObject.GetInstanceID()) return;
            if (condition)
            {
                Debug.LogWarning("Changing Color");
                meshRenderer.material.DOFloat(1, "_OutlineWidth", 1);
            }
            else
            {
                meshRenderer.material.DOFloat(0, "_OutlineWidth", 1);
            }
        }
        

        private void UnSubscribeEvents()
        {
            InteractableSignals.Instance.onChangeColorOfInteractableObject -= OnChangeColorOfInteractableObject;
            InteractableSignals.Instance.onDropTheInteractableObject -= OnDropTheInteractableObject;
            InteractableSignals.Instance.onPickUpTheInteractableObject -= OnPickUpTheInteractableObject;
            InteractableSignals.Instance.onDropandPickUpTheInteractableObject -= OnDropandPickUpTheInteractableObject;
            InteractableSignals.Instance.onInteractableOpenDoor -= OnInteractableOpenDoor;
        }

        

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        
    }
}