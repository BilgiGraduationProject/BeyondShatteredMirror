using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Managers;
using Runtime.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using Runtime.Signals;
using Sirenix.OdinInspector;

namespace Runtime.Controllers.UI
{
    public class InventoryPanelController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [Header("User Area Settings")]
        [SerializeField] private Image userAvatarImage;
        [SerializeField] private TextMeshProUGUI userNameText;
        
        [Header("Emotion Area Settings")]
        [SerializeField] private Animator emotionAnimator;
        
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
        private ItemData _selectedItemData;
        private List<Texture2D> _captureData = new List<Texture2D>();
        
        private List<GameObject> _shopItemObjects = new List<GameObject>();
        
        #endregion

        #endregion
        
        private void Awake()
        {
            if(!GameDataManager.HasData(GameDataEnums.Soul.ToString()))
            {
                GameDataManager.SaveData<int>(GameDataEnums.Soul.ToString(), 1);
            }
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
                if (GameDataManager.HasData(item.DataType.ToString()))
                {
                    print(item.DataType + " : " + GameDataManager.LoadData<int>(item.DataType.ToString()));
                    GameObject itemObject = Instantiate(shopItemObjectPrefab, shopItemObjectParent);
                    _shopItemObjects.Add(itemObject);
                    if (_shopItemObjects.Count is 1) _selectedItemData = item;
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
                    //itemObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = SaveLoadManager.Instance.LoadData<int>(item.Name).ToString();
                    itemObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = GameDataManager.LoadData<int>(item.DataType.ToString()).ToString();
                }
            }
            
            //Selected first one
            if (_shopItemObjects.Count > 0 && _shopItemData.Count > 0)
            {
                _shopItemObjects[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;
                selectedImage.sprite = _selectedItemData.Thumbnail;
                descriptionText.text = _selectedItemData.Description;
            }
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

        public void ClosePanel()
        {
            CoreUISignals.Instance.onClosePanel?.Invoke(1);
        }

        [Button("Change Emotion Animation State")]
        public void ChangeEmotionAnimationState(int num)
        {
            emotionAnimator.SetInteger("EmotionState", num);
            emotionAnimator.SetFloat("",2);
            print("sa");
        }
    }
}