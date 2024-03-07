using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class DragAndDropHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private ScrollRect scrollRect;

        private void Start()
        {
            scrollRect = transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect>();
            print(scrollRect.gameObject.name);
        }

        public void OnDrag(PointerEventData eventData)
        {
            scrollRect.OnDrag(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            scrollRect.OnBeginDrag(eventData);
            print("Begin Drag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            scrollRect.OnEndDrag(eventData);
        }
    }
}