using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Utilities;

namespace Runtime.Controllers.UI
{
    public class StartPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<EventTrigger> buttons = new List<EventTrigger>();
        [SerializeField] private GameObject buttonBackground;

        #endregion

        #region Private 

        private GameObject _currentButton;

        #endregion
        
        #endregion

        private void Awake()
        {
            foreach (var eventTrigger in buttons) AddPointerEnterAndExit(eventTrigger);
        }

        private void Start()
        {
            buttons[0].OnPointerEnter(new PointerEventData(EventSystem.current));
            buttonBackground.GetComponent<CanvasGroup>()
                .DOFade(.8f, 1f)
                .SetEase(Ease.InOutQuint)
                .SetLoops(-1);    
        }

        private void AddPointerEnterAndExit(EventTrigger button)
        {
            AddCallback(button, EventTriggerType.PointerEnter, OnPointerEnter);
            AddCallback(button, EventTriggerType.PointerExit, OnPointerExit);
            //AddCallback(button, EventTriggerType.PointerClick, OnPointerClick);
        }

        private void AddCallback(EventTrigger button, EventTriggerType eventType, UnityAction<BaseEventData> callback)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(callback);
            button.triggers.Add(entry);
        }

        private void OnPointerEnter(BaseEventData data)
        {
            if (((PointerEventData)data).pointerCurrentRaycast.gameObject is not null)
            {
                _currentButton = ((PointerEventData)data).pointerCurrentRaycast.gameObject;
                buttonBackground.transform.position = ((PointerEventData)data).pointerCurrentRaycast.gameObject.transform.position;
                ((PointerEventData)data).pointerCurrentRaycast.gameObject.transform
                    .DOScale(new Vector3().SetFloat(1.2f), .3f)
                    .SetEase(Ease.Flash);
            }
            else
            {
                _currentButton = buttons[0].transform.GetChild(0).gameObject;
                buttonBackground.transform.position = buttons[0].transform.GetChild(0).gameObject.transform.position;
                buttons[0].transform.GetChild(0).gameObject.transform
                    .DOScale(new Vector3().SetFloat(1.2f), .3f)
                    .SetEase(Ease.Flash);
            }
        }

        private void OnPointerExit(BaseEventData data)
        {
            _currentButton.transform
                .DOScale(Vector3.one, .3f)
                .SetEase(Ease.OutFlash);
        }
    }
}