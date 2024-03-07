using System.Collections.Generic;
using Runtime.Enums.UI;
using Runtime.Signals;
using UnityEngine;
using DG.Tweening;
using Runtime.Enums.GameManager;
using Runtime.Utilities;
using UnityEngine.Rendering;

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

        /*
        private void OnClosePanel(int layerValue)
        {
            // This helps to enable just for second the previous panel. Ayarlar acilinca sorun olmasın diye yapilan bir ayar.
            if(layers.Count - 1 == layerValue && layers[layerValue - 1].transform.childCount > 0)
            {
                layers[layerValue - 1].transform.GetChild(0).gameObject.SetActive(true);
            }
            
            else if (layers.Count - 2 == layerValue && layers[layerValue - 1].transform.childCount > 0) // TODO: Kontrolu GameManager ile yap.
            {
                if (layers[layerValue - 1].transform.GetChild(0).name.Contains(UIPanelTypes.Ingame.ToString()))
                {
                    layers[layerValue - 1].transform.GetChild(0).gameObject.SetActive(true); 
                }
            }
            
            //InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
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
        */
        
        private void OnClosePanel(int layerValue)
        {
            // Check if the layer is the last one or the one before the last one
            if (IsLastOrBeforeLastLayer(layerValue))
            {
                // Enable the previous panel
                EnableDisablePreviousPanel(true, layerValue);
            }

            // Close the current layer
            CloseCurrentLayer(layerValue);
        }

        private bool IsLastOrBeforeLastLayer(int layerValue)
        {
            return layers.Count - 1 == layerValue || layers.Count - 2 == layerValue;
        }

        private void EnableDisablePreviousPanel(bool value, int layerValue)
        {
            if (layers[layerValue - 1].transform.childCount > 0)
            {
                layers[layerValue - 1].transform.GetChild(0).gameObject.SetActive(value);
            }
        }

        private void CloseCurrentLayer(int layerValue)
        {
            if (layers[layerValue].transform.childCount > 0)
            {
                for (int i = 0; i < layers[layerValue].transform.childCount; i++)
                {
                    GameObject myObj = layers[layerValue].transform.GetChild(i).gameObject;
                    myObj.transform.localScale = Vector3.one;
                    myObj.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        Destroy(myObj);
                    });
                }
            }
        }
        
        
        /*
        private void OnOpenPanel(UIPanelTypes panel, short layerValue)
        {
            // This helps to disable just for second the previous panel. Ayarlar acilinca sorun olmasın diye yapilan bir ayar.
            if(layers.Count - 1 == layerValue && layers[layerValue - 1].transform.childCount > 0) 
            {
                layers[layerValue - 1].transform.GetChild(0).gameObject.SetActive(false); 
            }
            else if (layers.Count - 2 == layerValue && layers[layerValue - 1].transform.childCount > 0) // TODO: Kontrolu GameManager ile yap.
            {
                if (layers[layerValue - 1].transform.GetChild(0).name.Contains(UIPanelTypes.Ingame.ToString()))
                {
                    layers[layerValue - 1].transform.GetChild(0).gameObject.SetActive(false); 
                }
            }
            
            if (layers[layerValue].transform.childCount > 0 && layerValue is not 0) // This helps to close the previous panel
            {
                OnClosePanel(layerValue);
                
                // This helps to prevent opening the same panel
                if (!layers[layerValue].transform.GetChild(0).name.Contains(panel.ToString())) 
                {
                    //CoreUISignals.Instance.onClosePanel?.Invoke(layerValue);
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    GameObject gom = Instantiate(Resources.Load<GameObject>($"Prefabs/UIPanels/{panel}Panel"), layers[layerValue].transform);
            
                    if (layerValue is 0) return;
                    gom.transform.localScale = Vector3.zero;
                    gom.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
                }
                return;
            }
            
            //CoreUISignals.Instance.onClosePanel?.Invoke(layerValue);
            InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
            GameObject go = Instantiate(Resources.Load<GameObject>($"Prefabs/UIPanels/{panel}Panel"), layers[layerValue].transform);
            
            if (layerValue is 0) return;
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }
        */
        
        private void OnOpenPanel(UIPanelTypes panel, short layerValue)
        {
            // Check if there is already a panel open in a higher layer
            if (IsPanelOpenInHigherLayer(layerValue))
            {
                // If there is, we don't open a new panel and return
                return;
            }
            
            // Check if the layer is the last one or the one before the last one
            if (IsLastOrBeforeLastLayer(layerValue))
            {
                // Disable the previous panel
                EnableDisablePreviousPanel(false, layerValue);
            }

            // Open the new panel
            OpenNewPanel(panel, layerValue);
        }
        
        private bool IsPanelOpenInLayer(int layerValue)
        {
            return layers[layerValue].transform.childCount > 0;
        }
        
        private bool IsPanelOpenInHigherLayer(int layerValue)
        {
            for (int i = layerValue + 1; i < layers.Count; i++)
            {
                if (layers[i].transform.childCount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void OpenNewPanel(UIPanelTypes panel, short layerValue)
        {
            if (layers[layerValue].transform.childCount > 0 && layerValue is not 0)
            {
                OnClosePanel(layerValue);
                if (!layers[layerValue].transform.GetChild(0).name.Contains(panel.ToString()))
                {
                    InstantiateNewPanel(panel, layerValue);
                }
            }
            else
            {
                InstantiateNewPanel(panel, layerValue);
            }
        }

        private void InstantiateNewPanel(UIPanelTypes panel, short layerValue)
        {
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // This is for closing the panels with the escape button
            {
                // Close the topmost panel
                CloseTopmostPanel();
            }
        }
        
        private void CloseTopmostPanel()
        {
            for (int i = layers.Count - 1; i > 0; i--)
            {
                if (layers[i].transform.childCount > 0)
                {
                    OnClosePanel(i);
                    break;
                }
            }
        }
    }
}