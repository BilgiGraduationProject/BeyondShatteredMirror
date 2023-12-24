using System.Collections.Generic;
using Runtime.Enums.UI;
using Runtime.Signals;
using UnityEngine;
using DG.Tweening;

namespace Runtime.Controllers.UI
{
    public class UIPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<GameObject> layers = new List<GameObject>();

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanels;
        }

        private void OnCloseAllPanels()
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.transform.childCount; i++)
                {
                    Destroy(layer.transform.GetChild(i).gameObject);
                    
                }
            }
        }

        private void OnClosePanel(int layerValue)
        {
            if (layers[layerValue].transform.childCount > 0)
            {
                for (int i = 0; i < layers[layerValue].transform.childCount; i++)
                {
                    Destroy(layers[layerValue].transform.GetChild(i).gameObject);
                    
                }
            }
        }

        private void OnOpenPanel(UIPanelTypes panel, short layerValue)
        {
           CoreUISignals.Instance.onClosePanel?.Invoke(layerValue);
           GameObject go = Instantiate(Resources.Load<GameObject>($"Prefabs/UIPanels/{panel}Panel"), layers[layerValue].transform);
           if (layerValue is 0) return;
           go.transform.localScale = Vector3.zero;
           go.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }

        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
            CoreUISignals.Instance.onClosePanel -= OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanels -= OnCloseAllPanels;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Start()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 0);
        }
    }
}