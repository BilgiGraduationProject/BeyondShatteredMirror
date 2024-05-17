using DG.Tweening;
using Runtime.Managers;
using Runtime.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace Runtime.Controllers.UI
{
    public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private Image itemThumbnail;
        [SerializeField] private TextMeshProUGUI itemPrice;
        [SerializeField] private Outline itemOutline;
        
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
            itemPrice.text = price + " Souls"; // Changeable
        }
        
        internal void SetItemIndex(int index) => _itemIndex = index;

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetOutlineAnimation();
            transform.DOScale(new Vector3().SetFloat(1.1f), 0.3f)
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
        
        [Button("Set Outline Animation")]
        private void SetOutlineAnimation()
        {
            itemOutline.effectDistance = new Vector2(50, 120);

            DOTween.To(() => itemOutline.effectDistance, w => itemOutline.effectDistance = w, new Vector2(50, 0), 0.5f)
                .SetEase(Ease.Flash)
                .OnComplete(() =>
                    DOTween.To(() => itemOutline.effectDistance, w => itemOutline.effectDistance = w,
                            new Vector2(0, 0), 0.5f)
                        .SetEase(Ease.Flash));
        }
    }
}