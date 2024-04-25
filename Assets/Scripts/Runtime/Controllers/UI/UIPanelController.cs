using System;
using System.Collections.Generic;
using Runtime.Enums.UI;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;
using DG.Tweening;
using Runtime.Enums.GameManager;
using Runtime.Managers;
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
            CoreUISignals.Instance.onEnableAllPanels += OnEnableAllPanels;
            CoreUISignals.Instance.onDisableAllPanels += OnDisableAllPanels;
        }
        
        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
            CoreUISignals.Instance.onClosePanel -= OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanels -= OnCloseAllPanels;
            CoreUISignals.Instance.onEnableAllPanels -= OnEnableAllPanels;
            CoreUISignals.Instance.onDisableAllPanels -= OnDisableAllPanels;
        }

        private void OnDisable() => UnSubscribeEvents();
        
        #endregion

        private void OnDisableAllPanels()
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.transform.childCount; i++)
                {
                    layer.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        private void OnEnableAllPanels()
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.transform.childCount; i++)
                {
                    layer.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        
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
            CoreUISignals.Instance.onPlaySFX?.Invoke(SFXTypes.ButtonOpen);
            
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
            
            if (layers[0].transform.childCount > 0 && layers[0].transform.GetChild(0).name.Replace("Panel(Clone)", "").Trim() 
                == UIPanelTypes.Start.ToString())
            {
                CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Start);
                print("Start panel is open. Game state is Start.");
            }
            
            if(!value) return;
            
            // Switch case for UIPanelTypes
            if (layers[layerValue - 1].transform.childCount > 0)
            {
                string panelName = layers[layerValue - 1].transform.GetChild(0).name.Replace("Panel(Clone)", "").Trim();
                print(panelName);
                UIPanelTypes panel = (UIPanelTypes)Enum.Parse(typeof(UIPanelTypes), panelName);
                print(panel);
                switch (panel)
                {
                    case UIPanelTypes.Start:
                        CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Start);
                        break;
                    case UIPanelTypes.Settings:
                        CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                        break;
                    case UIPanelTypes.Quit:
                        CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Quit);
                        break;
                    case UIPanelTypes.Ingame:
                        CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
                        break;
                    case UIPanelTypes.Inventory:
                        CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                        break;
                    case UIPanelTypes.Pause:
                        CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                        break;
                    case UIPanelTypes.Shop:
                        CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                        break;
                    default:
                        break;
                }
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
        
        /// <summary>
        /// Opens a new UI panel based on the given UIPanelTypes enum value.
        /// This function checks if a cutscene is playing and prevents opening a new panel during a cutscene.
        /// It also checks if there is already a panel open in a higher layer and prevents opening a new panel in such cases.
        /// If the layer is the last one or the one before the last one, it disables the previous panel.
        /// </summary>
        /// <param name="panel">The type of the panel to open.</param>
        /// <param name="layerValue">The layer value to determine where to open the panel.</param>
        ///
        /// /// <summary>
        /// Verilen UIPanelTypes enum değerine göre yeni bir UI paneli açar.
        /// Bu fonksiyon bir cutscene'in oynatılıp oynatılmadığını kontrol eder ve bir cutscene sırasında yeni bir panel açılmasını engeller.
        /// Ayrıca, daha yüksek bir katmanda zaten bir panelin açık olup olmadığını kontrol eder ve bu tür durumlarda yeni bir panel açılmasını engeller.
        /// Katman sonuncu veya sonuncudan bir önceki ise, önceki paneli devre dışı bırakır.
        /// </summary>
        /// <param name="panel">Açılacak panelin tipi.</param>
        /// <param name="layerValue">Panelin nerede açılacağını belirlemek için katman değeri.</param>
        private void OnOpenPanel(UIPanelTypes panel, short layerValue)
        {
            // Check if a cutscene is playing. If it is, we don't open a new panel.
            if (CoreGameSignals.Instance.onGetGameState() is GameStateEnum.Cutscene && layerValue > 0)
            {
                print($"Cutscene is playing. You can't open a panel right now. \n GameState: {CoreGameSignals.Instance.onGetGameState()}");
                return;
            }
            
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
            
            // Switch case for UIPanelTypes
            switch (panel)
            {
                case UIPanelTypes.Start:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Start);
                    break;
                case UIPanelTypes.Settings:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                    break;
                case UIPanelTypes.Quit:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Quit);
                    break;
                case UIPanelTypes.Ingame:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
                    CoreUISignals.Instance.onPlaySFX?.Invoke(SFXTypes.ButtonOpen);
                    break;
                case UIPanelTypes.Inventory:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                    CoreUISignals.Instance.onPlaySFX?.Invoke(SFXTypes.ButtonOpen);
                    break;
                case UIPanelTypes.Pause:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                    CoreUISignals.Instance.onPlaySFX?.Invoke(SFXTypes.ButtonOpen);
                    break;
                case UIPanelTypes.Shop:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                    CoreUISignals.Instance.onPlaySFX?.Invoke(SFXTypes.ButtonOpen);
                    break;
                default:
                    break;
            }
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
            
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // This is for closing the panels with the escape button
            {
                // Close the topmost panel
                CloseTopmostPanel();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
               
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