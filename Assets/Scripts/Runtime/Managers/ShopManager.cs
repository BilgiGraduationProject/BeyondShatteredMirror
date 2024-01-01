using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Runtime.Controllers.UI;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.UI;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class ShopManager : MonoBehaviour
    {
        #region Singleton
    
        public static ShopManager Instance;
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    
        #endregion
        
        #region Self Variables

        #region Private Variables

        private const string _pathOfData = "Data/ItemDatas";
        private List<ItemData> _itemDatas = new List<ItemData>();

        #endregion

        #endregion
        
        #region SubscribeEvents and UnsubscribeEvents

        private void OnEnable() => SubscribeEvents();

        private void SubscribeEvents()
        {
            
        }

        private void UnsubscribeEvents()
        {
            
        }

        private void OnDisable() => UnsubscribeEvents();

        #endregion

        private void Start()
        {
            _itemDatas = GetItemDatas();
        }
        
        private List<ItemData> GetItemDatas()
        {
            return new List<ItemData>(Resources.LoadAll<CD_Item>(_pathOfData)
                .Select(item => item.Data)
                .ToList());
        }

        public List<ItemData> SendItemDatasToControllers()
        {
            return _itemDatas;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Shop, 1);
            }
        }
        
        public void PurchaseItem(int itemIndex, GameObject itemObject)
        {
            if(_itemDatas is null) return;
            if(_itemDatas.Count < itemIndex) return;
            
            Debug.Log("Purchased item: " + _itemDatas[itemIndex].Name);

            if (CanAfford(_itemDatas[itemIndex].Price))
            {
                ConfirmPurchase(itemObject ,_itemDatas[itemIndex].Price);
            }
            else
            {
                ErrorPurchase(itemObject);
            }
        }
        
        private bool CanAfford(int itemPrice)
        {
            int currentSoul = SaveLoadManager.Instance.LoadData<int>(SaveDataValues.Soul);
            return itemPrice <= currentSoul;
        }
        
        private void ConfirmPurchase(GameObject itemObject ,int cost)
        {
            SaveLoadManager.Instance.ArithmeticalData(SaveDataValues.Soul, -cost);
            itemObject.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.5f)
                .SetEase(Ease.InOutBack);
            print("ConfirmPurchase");
        }
        
        private void CancelPurchase()
        {
            print("CancelPurchase");
        }
        
        private void ErrorPurchase(GameObject itemObject)
        {
            print("ErrorPurchase");
            itemObject.transform.DOShakePosition(0.5f, 20f);
        }
        
        private void AddItemToInventory()
        {
            print("AddItemToInventory");
        }
    }
}