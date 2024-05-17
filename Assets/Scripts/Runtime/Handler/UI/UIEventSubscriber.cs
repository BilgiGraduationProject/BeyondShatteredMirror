using System;
using Runtime.Enums.UI;
using Runtime.Managers;
using Runtime.Signals;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (subscriptionType)
            {
                case UIEventSubscriptionTypes.OnDefault:
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    break;
                case UIEventSubscriptionTypes.OnStart:
                    print("OnStart");
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    _uiManager.OnStart();
                    break;
                case UIEventSubscriptionTypes.OnSettings:
                    print("OnSettings");
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    _uiManager.OnSettings();
                    break;
                case UIEventSubscriptionTypes.OnQuit:
                    print("OnQuit");
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonClose);
                    _uiManager.OnQuit();
                    break;
                case UIEventSubscriptionTypes.OnMainMenu:
                    print("OnMainMenu");
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    _uiManager.OnMainMenu();
                    break;
                case UIEventSubscriptionTypes.OnShop:
                    _uiManager.OnShop();
                    break;
                case UIEventSubscriptionTypes.OnInventory:
                    _uiManager.OnInventory();
                    break;
            }
        }
    }
}