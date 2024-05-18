using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Runtime.Controllers.UI;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Enums.UI;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Utilities;
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

        public List<ItemData> SendItemDatasToControllers() => _itemDatas;

        private void Update()
        {
            
        }
        
        public void PurchaseItem(int itemIndex, GameObject itemObject)
        {
            if(_itemDatas is null) return;
            if(_itemDatas.Count < itemIndex) return;
            
            if (CanAfford(_itemDatas[itemIndex].Price)) ConfirmPurchase(itemObject, itemIndex);
            else ErrorPurchase(itemObject);
        }
        
        private bool IsPassesCommon(ItemData data) // It is not necessary for now. Maybe it will be usable in the future.
            => data switch
            {
                { Name: "Shield"} => true,
                { Price: >= 0 } and {Price: <= 2000 } => true,
                _ => false
            };
        
        private bool CanAfford(int itemPrice)
        {
            int currentSoul = GameDataManager.LoadData<int>(GameDataEnums.Soul.ToString());
            return itemPrice <= currentSoul;
        }
        
        private void ConfirmPurchase(GameObject itemObject, int itemIndex)
        {
            //SaveLoadManager.Instance.ArithmeticalData(GameDataKeys.Soul, -_itemDatas[itemIndex].Price);
            //SaveLoadManager.Instance.ArithmeticalData(_itemDatas[itemIndex].Name, +1);
            
            int currentSoul = GameDataManager.LoadData<int>(GameDataEnums.Soul.ToString());
            GameDataManager.SaveData(GameDataEnums.Soul.ToString(), currentSoul - _itemDatas[itemIndex].Price);

            int currentItemQuantity = GameDataManager.LoadData<int>(_itemDatas[itemIndex].DataType.ToString());
            GameDataManager.SaveData(_itemDatas[itemIndex].DataType.ToString(), currentItemQuantity + 1);
            itemObject.transform.DOScale(new Vector3().SetFloat(1.25f), 0.5f)
                .SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    itemObject.transform.DOScale(Vector3.one, 0.3f)
                        .SetEase(Ease.InOutBack);
                });
            print("ConfirmPurchase".ColoredText(Color.green));
        }
        
        private void CancelPurchase()
        {
            print("CancelPurchase");
        }
        
        private void ErrorPurchase(GameObject itemObject)
        {
            print("ErrorPurchase".ColoredText(Color.red));
            itemObject.transform.DOShakePosition(0.5f, 20f);
        }
    }
}