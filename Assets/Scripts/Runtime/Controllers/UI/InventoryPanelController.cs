using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Managers;
using Runtime.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using Runtime.Signals;

namespace Runtime.Controllers.UI
{
    public class InventoryPanelController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [Header("User Area Settings")]
        [SerializeField] private Image userAvatarImage;
        [SerializeField] private TextMeshProUGUI userNameText;
        
        [Header("Health Area Settings")]
        [SerializeField] private GameObject healthImage;
        
        [Header("Weapon Area Settings")]
        [SerializeField] private Image weaponImage;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        
        [Header("Selected Area Settings")]
        [SerializeField] private Image selectedImage;
        
        [Header("Description Area Settings")]
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        [Header("Buttons Area Settings")]
        [SerializeField] private List<EventTrigger> buttons = new List<EventTrigger>();
        
        [Header("Shop Items Area Settings")]
        [Header("Shop Item Objects Area Settings")]
        [SerializeField] private Transform shopItemObjectParent;
        [SerializeField] private GameObject shopItemObjectPrefab;
        
        [Header("Capture Objects Area Settings")]
        [SerializeField] private Transform captureObjectParent;
        [SerializeField] private GameObject captureObjectPrefab;
        
        #endregion
        
        #region Private Variables

        private Texture2D[] _captureTextureArray;
        private List<ItemData> _shopItemData = new List<ItemData>();
        private List<Texture2D> _captureData = new List<Texture2D>();
        
        private List<GameObject> _shopItemObjects = new List<GameObject>();
        
        #endregion

        #endregion
        
        private void Awake()
        {
            //if(_photoSpriteList.Count > 0) _photoSpriteList.Clear();
            GetShopItemDatas();
            GetCaptureDatas();
            Initialize();
        }

        private void Start()
        {
            GetShopItemObjects();
            GetCaptureObjects();
        }

        private void Initialize()
        {
            AddCallback(buttons[0], EventTriggerType.PointerDown, OnPointerClick);
        }
        
        private void AddCallback(EventTrigger button, EventTriggerType eventType, UnityAction<BaseEventData> callback)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(callback);
            button.triggers.Add(entry);
        }
        
        private void OnPointerClick(BaseEventData data)
        {
            CoreUISignals.Instance.onClosePanel?.Invoke(1);
        }

        private void GetShopItemDatas()
        {
            _shopItemData = ShopManager.Instance.SendItemDatasToControllers();
        }
        
        private void GetCaptureDatas()
        {
            _captureTextureArray = Resources.LoadAll<Texture2D>("CapturePhotos");

            foreach (var photo in _captureTextureArray)
            {
                _captureData.Add(photo);
            }
        }
        
        private void GetShopItemObjects()
        {
            if(_shopItemData.Count is 0) return;
            
            foreach (var item in _shopItemData)
            {
                if (ES3.KeyExists(item.Name))
                {
                    print(item.Name + " : " + SaveLoadManager.Instance.LoadData<int>(item.Name));
                    GameObject itemObject = Instantiate(shopItemObjectPrefab, shopItemObjectParent);
                    _shopItemObjects.Add(itemObject);
                    itemObject.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                    {
                        DisableItemsToggle();
                        if (value)
                        {
                            itemObject.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                            selectedImage.sprite = item.Thumbnail;
                            descriptionText.text = item.Description;
                        }
                    });
                    //itemObject.gameObject.GetComponent<Toggle>().onValueChanged.AddListener((value => DisableItemsToggle()));
                    itemObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = item.Thumbnail;
                    itemObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = SaveLoadManager.Instance.LoadData<int>(item.Name).ToString();
                }
            }
            
            //Selected first one
            _shopItemObjects[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;
            selectedImage.sprite = _shopItemData[0].Thumbnail;
            descriptionText.text = _shopItemData[0].Description;
        }

        private void GetCaptureObjects()
        {
            if(_captureData.Count is 0) return;
            
            foreach (var photo in _captureData)
            {
                GameObject photoObject = Instantiate(captureObjectPrefab, captureObjectParent);
                //photoObject.GetComponent<Image>().sprite = photo;
                photoObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RawImage>().texture = photo;
            }
        }

        private void DisableItemsToggle()
        {
            _shopItemObjects.ForEach(item =>
            {
                item.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                item.GetComponent<Toggle>().isOn = false;
            });
        }
    }
}