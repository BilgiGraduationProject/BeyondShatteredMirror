using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers
{
    public class PillController : MonoBehaviour
    {
        #region Self Variables

        #region Public  Variables

        public PillTypes PillType;
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

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _itemData = ShopManager.Instance.SendItemDatasToControllers();
        }

        private void Start()
        {
            PillImage.sprite = _itemData.Find(item => item.DataType.ToString() == PillType.ToString()).Thumbnail;
        }

        public void ConsumePill()
        {
            int currentPill = GameDataManager.LoadData<int>(PillType.ToString(), 0);
            if(currentPill == 0) return;
            GameDataManager.SaveData<int>(PillType.ToString(), currentPill - 1);
        }
        
        public void EffectPill()
        {
            switch (PillType)
            {
                case PillTypes.AntiDepressantPill:
                    //PlayerSignals.Instance.onTakeHealth?.Invoke(1);
                    break;
                case PillTypes.HealthPill:
                    //PlayerSignals.Instance.onIncreaseDamage?.Invoke(1);
                    break;
                case PillTypes.PsychoPill:
                    //PlayerSignals.Instance.onIncreaseSpeed?.Invoke(1);
                    break;
                case PillTypes.PulseofImmortalityPerk:
                    break;
                case PillTypes.SalvageSavior:
                    break;
                case PillTypes.Shield:
                    break;
                case PillTypes.SonicPerk:
                    break;
                case PillTypes.SoulHarvestAmplifier:
                    break;
            }
        }
        
        public void OnMousePill()
        {
            _selectedItemData = _itemData.Find(item => item.DataType.ToString() == PillType.ToString());
            UITextSignals.Instance.onSetPillDescription?.Invoke(_selectedItemData.Description);
        }
    }
}