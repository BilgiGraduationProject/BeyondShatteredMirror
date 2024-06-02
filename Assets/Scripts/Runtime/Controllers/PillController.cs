using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers
{
    public class PillController : MonoBehaviour
    {
        #region Self Variables

        #region Public  Variables

        public PillTypes PillType;
        public TextMeshProUGUI PillCountText;
        public Image PillBGImage;
        public Image PillImage;

        #endregion

        #region Serialized Variables

        //

        #endregion

        #region Private Variables

        private List<ItemData> _itemData = new List<ItemData>();
        private ItemData _selectedItemData;
        
        #endregion

        #endregion
        
        private void OnEnable()
        {
            Initialize();
            PillImage.sprite = _itemData.Find(item => item.DataType.ToString() == PillType.ToString()).Thumbnail; 
            //PillManager.Instance.Skills.Find(item => item.PillType == PillType);
            int currentPill = GameDataManager.LoadData<int>(PillType.ToString(), 0);
            PillCountText.text = currentPill.ToString();
            if (currentPill == 0)
                PillBGImage.color = Color.black;
            else if (PillManager.Instance.Skills.Find(item => item.PillType == PillType).IsActive)
                PillBGImage.color = Color.Lerp(Color.red, Color.yellow, .5f);
            else
                PillBGImage.color = Color.white;
        }
        
        private void Initialize()
        {
            _itemData = ShopManager.Instance.SendItemDatasToControllers();
        }

        public void ConsumePill()
        {
            
        }
        
        public void EffectPill()
        {
            if (PillManager.Instance.Skills.Find(item => item.PillType == PillType).IsActive) return;
            
            int currentPill = GameDataManager.LoadData<int>(PillType.ToString(), 0);
            
            print(GameDataManager.LoadData<int>(PillType.ToString(), 0));
            
            if(currentPill == 0) return;
            GameDataManager.SaveData<int>(PillType.ToString(), currentPill - 1);
            
            print(GameDataManager.LoadData<int>(PillType.ToString(), 0));
            
            PlayerSignals.Instance.onSetPillEffect?.Invoke(PillType);
            
        }
        
        public void OnMousePill()
        {
            _selectedItemData = _itemData.Find(item => item.DataType.ToString() == PillType.ToString());
            UITextSignals.Instance.onSetPillDescription?.Invoke(_selectedItemData.Description);
        }
    }
}