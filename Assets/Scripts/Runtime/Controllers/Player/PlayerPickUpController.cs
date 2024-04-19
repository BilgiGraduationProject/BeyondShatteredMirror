using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Runtime.Enums.Collectable;
using Runtime.Enums.Player;
using Runtime.Signals;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPickUpController : MonoBehaviour
    {

        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerHandTransform;
        
        
        

        #endregion


        #region Private Variables

        private CollectableEnum _collectableType;

        #endregion

        #endregion
        public void OnPlayerStartToPickUp(GameObject collectableObj)
        {
            if (Vector3.Distance(playerTransform.position, collectableObj.transform.position) > 2f) return;
            CollectableSignals.Instance.onCheckCollectableType?.Invoke(collectableObj);
            CollectableCollect(_collectableType, collectableObj);
            
        }

        private void CollectableCollect(CollectableEnum collectableType, GameObject collectableObj)
        {
           
            Debug.LogWarning(collectableObj.name + collectableType.ToString());
            switch (collectableType)
            {
                case CollectableEnum.Open:
                    var newPos = new Vector3(collectableObj.transform.position.x,transform.position.y, transform.position.z);
                    playerTransform.DOLookAt(newPos, 1f);
                    CollectableSignals.Instance.onCollectableDoJob?.Invoke(collectableObj,collectableType);
                    break;
                case CollectableEnum.Carryable:
                    if (playerHandTransform.childCount > 0)
                    {
                        playerHandTransform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                        playerHandTransform.GetChild(0).transform.parent = null;
                        
                        
                    }
                    CollectableSignals.Instance.onCollectableDoJob?.Invoke(collectableObj,collectableType);
                    collectableObj.transform.parent = playerHandTransform;
                    collectableObj.transform.localPosition = Vector3.zero;
                    break;
                
                case CollectableEnum.Puzzle:
                    if (playerHandTransform.childCount > 0)
                    {
                        CollectableSignals.Instance.onCollectableDoJob?.Invoke(collectableObj,collectableType);
                        
                    }
                    
                   
                    break;
                
            }
        }

        public void OnSendCollectableType(CollectableEnum collectableType) => _collectableType = collectableType;

        public void OnPlayerPressedDropItemButton()
        {
            if (playerHandTransform.childCount > 0)
            {
                playerHandTransform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                playerHandTransform.GetChild(0).transform.parent = null;
            }
        }

        public GameObject onSendPlayerItemTag()
        {
            return playerHandTransform.GetChild(0).gameObject;
        }
    }
}