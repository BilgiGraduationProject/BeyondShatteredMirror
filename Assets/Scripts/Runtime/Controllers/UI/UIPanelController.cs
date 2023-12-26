using System.Collections.Generic;
using Runtime.Enums.UI;
using Runtime.Signals;
using UnityEngine;
using DG.Tweening;
using Runtime.Enums.GameManager;

namespace Runtime.Controllers.UI
{
    public class UIPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<GameObject> layers = new List<GameObject>();

        #endregion

        #endregion

        #region SubscribeEvents and UnsubscribeEvents
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanels;
        }
        
        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
            CoreUISignals.Instance.onClosePanel -= OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanels -= OnCloseAllPanels;
        }

        private void OnDisable() => UnSubscribeEvents();
        
        #endregion

        private void OnCloseAllPanels()
        {
            InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
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
            InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
            if (layers[layerValue].transform.childCount > 0)
            {
                for (int i = 0; i < layers[layerValue].transform.childCount; i++)
                {
                    //Destroy(layers[layerValue].transform.GetChild(i).gameObject);
                    GameObject myObj = layers[layerValue].transform.GetChild(i).gameObject;
                    
                    // TODO: This part is for testing purposes only. Change it later with animation.
                    if(layerValue is 0) return;
                    myObj.transform.localScale = Vector3.one;
                    myObj.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        Destroy(myObj);
                    });
                }
            }
        }

        private void OnOpenPanel(UIPanelTypes panel, short layerValue)
        {
            if (layers[layerValue].transform.childCount > 0 && layerValue is not 0) // This helps to close the previous panel
            {
                OnClosePanel(layerValue);
                return;
            }
            
            //CoreUISignals.Instance.onClosePanel?.Invoke(layerValue);
            InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
            GameObject go = Instantiate(Resources.Load<GameObject>($"Prefabs/UIPanels/{panel}Panel"), layers[layerValue].transform);
            
            if (layerValue is 0) return;
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }

        private void Start()
        {
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Cutscene);
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 0);
        }
    }
}