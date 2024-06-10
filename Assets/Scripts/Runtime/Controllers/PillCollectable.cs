using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers
{
    public class PillCollectable: MonoBehaviour
    {
        [SerializeField] private PillTypes pillType;

        public void CollectPill()
        {
            CoreUISignals.Instance.onPillCollected?.Invoke(pillType);
            var currentItemQuantity = GameDataManager.LoadData<int>(pillType.ToString());
            GameDataManager.SaveData(pillType.ToString(), currentItemQuantity + 1);
            gameObject.SetActive(false);
        }
    }
}