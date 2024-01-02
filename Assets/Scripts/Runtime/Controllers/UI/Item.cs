using DG.Tweening;
using Runtime.Managers;
using Runtime.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Runtime.Controllers.UI
{
    public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private Image itemThumbnail;
        [SerializeField] private TextMeshProUGUI itemPrice;

        #endregion
        
        #region Private Variables
        
        private int _itemIndex;
        
        #endregion

        #endregion
        
        public void SetItem(string name, string description, Sprite thumbnail, int price)
        {
            itemName.text = name;
            itemDescription.text = description;
            itemThumbnail.sprite = thumbnail;
            itemPrice.text = "$" + price; // Changeable
        }
        
        internal void SetItemIndex(int index) => _itemIndex = index;

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(new Vector3().SetFloat(1.15f), 0.3f)
                .SetEase(Ease.Flash);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.Flash);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ShopManager.Instance.PurchaseItem(_itemIndex, gameObject);
        }
    }
}