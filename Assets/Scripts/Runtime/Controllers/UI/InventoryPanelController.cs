using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Managers;
using Runtime.Utilities;
using TMPro;

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
        
        [Header("Items Area Settings")]
        [Header("Item Objects Area Settings")]
        [SerializeField] private Transform itemObjectParent;
        [SerializeField] private GameObject itemObjectPrefab;
        
        [Header("Capture Objects Area Settings")]
        [SerializeField] private Transform captureObjectParent;
        [SerializeField] private GameObject captureObjectPrefab;
        
        #endregion
        
        #region Private Variables

        private Texture2D[] _pcaptureTextureArray;
        private List<Texture2D> _captureData = new List<Texture2D>();
        private List<ItemData> _itemData = new List<ItemData>();
        
        private List<GameObject> _itemObjects = new List<GameObject>();
        
        #endregion

        #endregion
        
        private void Awake()
        {
            //if(_photoSpriteList.Count > 0) _photoSpriteList.Clear();
            GetItemDatas();
            GetCaptureDatas();
        }

        private void Start()
        {
            GetItemObjects();
            GetCaptureObjects();
        }

        private void GetItemDatas()
        {
            _itemData = ShopManager.Instance.SendItemDatasToControllers();
        }
        
        private void GetCaptureDatas()
        {
            _pcaptureTextureArray = Resources.LoadAll<Texture2D>("CapturePhotos");

            foreach (var photo in _pcaptureTextureArray)
            {
                _captureData.Add(photo);
            }
        }
        
        private void GetItemObjects()
        {
            foreach (var item in _itemData)
            {
                if (ES3.KeyExists(item.Name))
                {
                    print(item.Name + " : " + SaveLoadManager.Instance.LoadData<int>(item.Name));
                    GameObject itemObject = Instantiate(itemObjectPrefab, itemObjectParent);
                    _itemObjects.Add(itemObject);
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
            _itemObjects[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;
            selectedImage.sprite = _itemData[0].Thumbnail;
            descriptionText.text = _itemData[0].Description;
        }

        private void GetCaptureObjects()
        {
            foreach (var photo in _captureData)
            {
                GameObject photoObject = Instantiate(captureObjectPrefab, captureObjectParent);
                //photoObject.GetComponent<Image>().sprite = photo;
                photoObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RawImage>().texture = photo;
            }
        }

        private void DisableItemsToggle()
        {
            _itemObjects.ForEach(item =>
            {
                item.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                item.GetComponent<Toggle>().isOn = false;
            });
        }
    }
}