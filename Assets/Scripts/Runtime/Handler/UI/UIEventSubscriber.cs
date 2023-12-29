using System;
using Runtime.Enums.UI;
using Runtime.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Handler.UI
{
    public class UIEventSubscriber : MonoBehaviour, IPointerClickHandler
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private UIEventSubscriptionTypes subscriptionType;
        [SerializeField] private EventTrigger triggerEvent;

        #endregion

        [ShowInInspector] private UIManager _uiManager;

        #endregion

        private void Awake()
        {
            FindReferences();
        }

        private void FindReferences()
        {
            _uiManager ??= FindObjectOfType<UIManager>();
            triggerEvent ??= GetComponent<EventTrigger>();
        }

        // private void OnEnable()
        // {
        //     SubscribeEvents();
        // }
        //
        // private void SubscribeEvents()
        // {
        //     switch (subscriptionType)
        //     {
        //         case UIEventSubscriptionTypes.OnStart:
        //             button.onClick.AddListener(_uiManager.OnStart);
        //             break;
        //         case UIEventSubscriptionTypes.OnSettings:
        //             button.onClick.AddListener(_uiManager.OnSettings);
        //             break;
        //         case UIEventSubscriptionTypes.OnQuit:
        //             button.onClick.AddListener(_uiManager.OnQuit);
        //             break;
        //     }
        // }
        //
        // private void UnSubscribeEvents()
        // {
        //     switch (subscriptionType)
        //     {
        //         case UIEventSubscriptionTypes.OnStart:
        //             button.onClick.RemoveListener(_uiManager.OnStart);
        //             break;
        //         case UIEventSubscriptionTypes.OnSettings:
        //             button.onClick.RemoveListener(_uiManager.OnSettings);
        //             break;
        //         case UIEventSubscriptionTypes.OnQuit:
        //             button.onClick.RemoveListener(_uiManager.OnQuit);
        //             break;
        //     }
        // }
        //
        // private void OnDisable()
        // {
        //     UnSubscribeEvents();
        // }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (subscriptionType)
            {
                case UIEventSubscriptionTypes.OnStart:
                    print("OnStart");
                    _uiManager.OnStart();
                    break;
                case UIEventSubscriptionTypes.OnSettings:
                    print("OnSettings");
                    _uiManager.OnSettings();
                    break;
                case UIEventSubscriptionTypes.OnQuit:
                    print("OnQuit");
                    _uiManager.OnQuit();
                    break;
            }
        }
    }
}