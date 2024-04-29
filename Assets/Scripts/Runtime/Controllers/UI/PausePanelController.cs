using System.Collections.Generic;
using Runtime.Signals;
using Runtime.Enums.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class PausePanelController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables
        
        [SerializeField] private List<GameObject> pauseButtons = new List<GameObject>();
        
        #endregion

        #endregion

        private void OnEnable()
        {
            
        }
    }
}