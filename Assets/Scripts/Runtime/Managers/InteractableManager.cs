using System;
using DG.Tweening;
using Runtime.Enums.Collectable;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class InteractableManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        

        #endregion


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
            InteractableSignals.Instance.onPlayerInteractWithPuzzlePart += OnPlayerInteractWithPuzzlePart;
        }

        private void OnPlayerInteractWithPuzzlePart(GameObject puzzlePlace,GameObject puzzlePart )
        {
            if (puzzlePlace.GetInstanceID() != gameObject.GetInstanceID() || puzzlePart is null) return;
            if (puzzlePlace.CompareTag(puzzlePart.tag))
            {
                puzzlePart.transform.parent = null;
                puzzlePart.transform.position = puzzlePlace.transform.position;
                puzzlePart.transform.rotation = puzzlePlace.transform.rotation;
                puzzlePart.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                puzzlePart.layer = 0;
                puzzlePlace.layer = 0;

            } 
            
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
            obj.transform.localPosition = Vector3.zero;
        }

        private void OnDropTheInteractableObject(GameObject obj, Transform playerHandTransform)
        {
            if(obj.GetInstanceID() != gameObject.GetInstanceID()) return;
            obj.transform.parent = null;
            rigidbody.useGravity = true;
        }

        private void OnChangeColorOfInteractableObject(bool condition, GameObject obj)
        {
            if(obj.GetInstanceID() != gameObject.GetInstanceID()) return;
            Debug.LogWarning("Change the material color");
            if (condition)
            {
                if (type == CollectableEnum.Puzzle)
                {
                    meshRenderer.material.DOFade(0.55f, "_BaseColor", 1);
                }
                meshRenderer.material.DOFloat(1, "_OutlineWidth", 1);
            }
            else
            {
                if (type == CollectableEnum.Puzzle)
                {
                    meshRenderer.material.DOFade(0f, "_BaseColor", 1);
                }
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