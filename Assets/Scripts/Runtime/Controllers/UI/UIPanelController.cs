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
using Unity.VisualScripting;
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

        /// <summary>
        /// Tüm panelleri devre dışı bırakır. Her katmandaki tüm çocuk nesneleri devre dışı bırakır.
        /// </summary>
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

        /// <summary>
        /// Tüm panelleri etkinleştirir. Her katmandaki tüm çocuk nesneleri etkinleştirir.
        /// </summary>
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
        
        /// <summary>
        /// Tüm panelleri kapatır. Her katmandaki tüm çocuk nesneleri yok eder.
        /// </summary>
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
        
        /// <summary>
        /// Belirtilen katmandaki paneli kapatır. Eğer kapatılan katman sonuncu veya sonuncudan bir önceki ise, önceki paneli etkinleştirir.
        /// </summary>
        /// <param name="layerValue">Kapatılacak panelin katman değeri.</param>
        private void OnClosePanel(int layerValue)
        {
            CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
            
            // Check if the layer is the last one or the one before the last one
            if (IsLayerGreaterThanZero(layerValue))
            {
                // Enable the previous panel
                SetPreviousPanelState(true, layerValue);
            }

            // Close the current layer
            CloseCurrentLayer(layerValue);
        }

        /// <summary>
        /// Belirtilen katman değerinin 0'dan büyük olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="layerValue">Kontrol edilecek katmanın değeri.</param>
        /// <returns>Eğer belirtilen katman değeri 0'dan büyükse true, değilse false döner.</returns>
        private bool IsLayerGreaterThanZero(int layerValue)
        {
            //return layers.Count - 1 == layerValue || layers.Count - 2 == layerValue;
            return layerValue > 0;
        }

        /// <summary>
        /// Belirtilen katmandaki önceki paneli etkinleştirir veya devre dışı bırakır.
        /// Eğer panel etkinleştiriliyorsa, oyun durumunu da günceller.
        /// </summary>
        /// <param name="value">Önceki panelin etkinleştirilip etkinleştirilmeyeceğini belirler.</param>
        /// <param name="layerValue">Etkinleştirilecek veya devre dışı bırakılacak panelin katman değeri.</param>
        private void SetPreviousPanelState(bool value, int layerValue)
        {
            if (layers[layerValue - 1].transform.childCount > 0)
            {
                layers[layerValue - 1].transform.GetChild(0).gameObject.SetActive(value);
            }
            
            // Check if layer[0] is Start panel and if it is open, change the game state to Start
            if (layers[0].transform.childCount > 0 && layers[0].transform.GetChild(0).name.Replace("Panel(Clone)", "").Trim() 
                == UIPanelTypes.Start.ToString())
            {
                CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Start);
                //print("Start panel is open. Game state is Start.");
            }
            
            //if(!value) return;
            
            // Switch case for UIPanelTypes
            if (layers[layerValue - 1].transform.childCount > 0)
            {
                string panelName = layers[layerValue - 1].transform.GetChild(0).name.Replace("Panel(Clone)", "").Trim();
                //print(panelName);
                UIPanelTypes panel = (UIPanelTypes)Enum.Parse(typeof(UIPanelTypes), panelName);
                //print(panel);
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

        /// <summary>
        /// Belirtilen katmandaki paneli kapatır. Paneli kapatırken bir animasyon oynatır ve animasyon bittiğinde paneli yok eder.
        /// </summary>
        /// <param name="layerValue">Kapatılacak panelin katman değeri.</param>
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
        
        /// <summary>
        /// Verilen UIPanelTypes enum değerine göre yeni bir UI paneli açar.
        /// Bu fonksiyon bir cutscene'in oynatılıp oynatılmadığını kontrol eder ve bir cutscene sırasında yeni bir panel açılmasını engeller.
        /// Ayrıca, daha yüksek bir katmanda zaten bir panelin açık olup olmadığını kontrol eder ve bu tür durumlarda yeni bir panel açılmasını engeller.
        /// Katman sonuncu veya sonuncudan bir önceki ise, önceki paneli devre dışı bırakır.
        /// </summary>
        /// <param name="panel">Açılacak panelin tipi.</param>
        /// <param name="layerValue">Panelin nerede açılacağını belirlemek için katman değeri.</param>
        private void OnOpenPanel(UIPanelTypes panel, short layerValue)
        {
            //CheckEmptyPanels();
            
            // Check if the layer value is out of range
            if (layerValue < 0 || layerValue >= layers.Count)
            {
                //print("Layer value is out of range.");
                throw new ArgumentOutOfRangeException(nameof(layerValue), "Layer value is out of range.");
                return;
            }
            
            // Check if a cutscene is playing. If it is, we don't open a new panel.
            if (GameState(GameStateEnum.Cutscene) && layerValue > 0 || GameState(GameStateEnum.Capture)
                || GameState(GameStateEnum.Start) && panel is not UIPanelTypes.Settings && panel is not UIPanelTypes.Ingame)
            {
                //print($"You can't open a panel right now. GameState: {CoreGameSignals.Instance.onGetGameState()}, Panel: {panel}");
                return;
            }
            
            // if (CoreGameSignals.Instance.onGetGameState() is GameStateEnum.Cutscene && layerValue > 0 
            //     || CoreGameSignals.Instance.onGetGameState() is GameStateEnum.Start && panel is not UIPanelTypes.Settings && panel is not UIPanelTypes.Ingame
            //     || CoreGameSignals.Instance.onGetGameState() is GameStateEnum.Capture)
            // {
            //     //print($"You can't open a panel right now. \n GameState: {CoreGameSignals.Instance.onGetGameState()}, \t Panel: {panel}");
            //     return;
            // }
            
            // Check if there is already a panel open in a higher layer
            // TODO: Aynı layerda başka bir panel açıkken, buna izin vermemek için bir kontrol yapılmalı.
            if (IsPanelOpenInHigherLayer(layerValue))
            {
                // If there is, we don't open a new panel and return
                //print("IsPanelOpenInHigherLayer");
                return;
            }
            
            // Check if there is already a panel open in the same layer
            if (IsLayerGreaterThanZero(layerValue))
            {
                // Disable the previous panel
                //print("IsLayerGreaterThanZero, true");
                SetPreviousPanelState(false, layerValue);
            }
            else
            {
                //print("IsLayerGreaterThanZero, false");
                //print($"There is no panel in the layer {layerValue} to close.");
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
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    break;
                case UIPanelTypes.Inventory:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    break;
                case UIPanelTypes.Pause:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    break;
                case UIPanelTypes.Shop:
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.UI);
                    CoreUISignals.Instance.onPlayOneShotSound?.Invoke(SFXTypes.ButtonOpen);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Belirtilen katmanda bir panelin açık olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="layerValue">Kontrol edilecek katmanın değeri.</param>
        /// <returns>Belirtilen katmanda bir panel açıksa true, değilse false döner.</returns>
        private bool IsPanelOpenInLayer(int layerValue)
        {
            return layers[layerValue].transform.childCount > 0;
        }
        
        /// <summary>
        /// Belirtilen katmandan daha yüksek bir katmanda bir panelin açık olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="layerValue">Kontrol edilecek katmanın değeri.</param>
        /// <returns>Belirtilen katmandan daha yüksek bir katmanda bir panel açıksa true, değilse false döner.</returns>
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

        /// <summary>
        /// Belirtilen UIPanelTypes enum değerine göre yeni bir panel açar.
        /// </summary>
        /// <param name="panel">Açılacak panelin tipi.</param>
        /// <param name="layerValue">Panelin nerede açılacağını belirlemek için katman değeri.</param>
        private void OpenNewPanel(UIPanelTypes panel, short layerValue)
        {
            if (layers[layerValue].transform.childCount > 0 && layerValue is not 0)
            {
                OnClosePanel(layerValue);
                //print($"Closed the panel in layer {layerValue} to open a new one.");
                if (!layers[layerValue].transform.GetChild(0).name.Contains(panel.ToString()))
                {
                    InstantiateNewPanel(panel, layerValue);
                    //print("InstantiateNewPanel");
                }
            }
            else
            {
                InstantiateNewPanel(panel, layerValue);
                //print("InstantiateNewPanel");
            }
        }

        /// <summary>
        /// Belirtilen UIPanelTypes enum değerine göre yeni bir panel örneği oluşturur ve belirtilen katmanda bu paneli açar.
        /// </summary>
        /// <param name="panel">Açılacak panelin tipi.</param>
        /// <param name="layerValue">Panelin nerede açılacağını belirlemek için katman değeri.</param>
        private void InstantiateNewPanel(UIPanelTypes panel, short layerValue)
        {
            InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
            GameObject go = Instantiate(Resources.Load<GameObject>($"Prefabs/UIPanels/{panel}Panel"), layers[layerValue].transform);
            if (layerValue is 0) return;
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }
        
        private bool GameState(GameStateEnum state)
        {
            return CoreGameSignals.Instance.onGetGameState() == state;
        }

        private void CheckEmptyPanels()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].transform.childCount is 0)
                {
                    //print($"Boş: {i}");
                    break;
                }
                else
                {
                    //print($"Dolu: {i}");
                }
            }
        }

        private void Awake()
        {
            
        }

        // #if !UNITY_EDITOR
        //
        // [RuntimeInitializeOnLoadMethod]
        // [Obsolete("Obsolete")]
        // static void SetScreenResolutionOnLoad()
        // {
        //     Resolution resolution = GameDataManager.LoadData<Resolution>(GameDataEnums.Resolution.ToString(),
        //         new Resolution
        //         {
        //             width = Screen.currentResolution.width,
        //             height = Screen.currentResolution.height,
        //             refreshRate = Screen.currentResolution.refreshRate
        //         });
        //     Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        // }
        //
        // #endif
        
        private void Start()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 0);
            CoreUISignals.Instance.onPlayMusic?.Invoke(SFXTypes.MainMenu);
            
            if(!GameDataManager.HasData(GameDataEnums.Soul.ToString()))
            {
                GameDataManager.SaveData<int>(GameDataEnums.Soul.ToString(), 1);
            }
            if(!GameDataManager.HasData(GameDataEnums.HealthPill.ToString()))
            {
                GameDataManager.SaveData<int>(GameDataEnums.HealthPill.ToString(), 1);
            }
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape) && GameState(GameStateEnum.Game)) // This is for opening the start panel
            {
                CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Pause, 1);
                return;
            }
            if (Input.GetKeyUp(KeyCode.P) && GameState(GameStateEnum.Game))
            {
                CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Pause, 1);
                return;
            }
            else if (Input.GetKeyUp(KeyCode.P) && GameState(GameStateEnum.UI))
            {
                CloseTheTopPanel();
            }
            if (Input.GetKeyUp(KeyCode.M) && GameState(GameStateEnum.Game))
            {
                CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Shop, 1);
                return;
            }
            else if (Input.GetKeyUp(KeyCode.M) && GameState(GameStateEnum.UI))
            {
                CloseTheTopPanel();
            }
            if (Input.GetKeyUp(KeyCode.I) && GameState(GameStateEnum.Game))
            {
                CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Inventory, 1);
                return;
            }
            else if (Input.GetKeyUp(KeyCode.I) && GameState(GameStateEnum.UI))
            {
                CloseTheTopPanel();
            }
            if (Input.GetKeyUp(KeyCode.Escape) && GameState(GameStateEnum.UI) && CheckBug())
            {
                CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
                return;
            }
            
            if (Input.GetKeyUp(KeyCode.Escape)) // This is for closing the panels with the escape button
            {
                // Close the topmost panel
                CloseTheTopPanel();
            }
        }

        private bool CheckBug()
        {
            for(int i = layers.Count - 1; i >= 0; i--)
            {
                if (layers[i].transform.childCount > 0)
                {
                    if (i == 0)
                    {
                        return true;
                        break;
                    }
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Escape tuşuna basıldığında en üstteki paneli kapatır.
        /// </summary>
        private void CloseTheTopPanel()
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