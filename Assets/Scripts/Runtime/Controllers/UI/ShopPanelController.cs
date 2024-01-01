using Runtime.Data.ValueObject;
using System.Collections.Generic;
using Runtime.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class ShopPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject shopItemPrefab;
        [SerializeField] private Transform shopItemParent;

        #endregion

        #region Private Variables

        private List<ItemData> _itemData = new List<ItemData>();

        #endregion

        #endregion

        private void Awake()
        {
            GetItemDatas();
            CreateShopItems();
        }

        //internal void GetItemDatas3(List<ItemData> itemDatas) => _itemData = itemDatas;

        private void GetItemDatas()
        {
            _itemData = ShopManager.Instance.SendItemDatasToControllers();
        }
        
        private void CreateShopItems()
        {
            int index = 0;
            foreach (var item in _itemData)
            {
                var itemObject = Instantiate(shopItemPrefab, shopItemParent.transform);
                itemObject.GetComponent<Item>().SetItem(item.Name, item.Description, item.Thumbnail, item.Price); 
                itemObject.GetComponent<Item>().SetItemIndex(index++);
                // itemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Name;
                // itemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Description;
                // itemObject.transform.GetChild(2).GetComponent<Image>().sprite = item.Thumbnail;
                // itemObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.Price.ToString();
            }
        }
    }
}