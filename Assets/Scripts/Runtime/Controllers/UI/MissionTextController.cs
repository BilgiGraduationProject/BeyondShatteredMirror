using System;
using Runtime.Data.UnityObject;
using Runtime.Enums.UI;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.UI
{
    public class MissionTextController : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private CD_UIMissionTextData _missionData;
        private string showText;

        #endregion

        #region Serialzied Variables

        [SerializeField] private TextMeshProUGUI missionText;

        #endregion

        #endregion
        public void SetMissionData(CD_UIMissionTextData missionData)
        {
            _missionData = missionData;
        }

        public void OnChangeMissionText(UITextEnum textEnum)
        {
            Debug.LogWarning("Text is Changed");
            var text = _missionData.data[(int)textEnum].text;
            switch (textEnum)
            {
                
                case UITextEnum.GoToMirror:
                    showText = text;
                    missionText.text = text;
                    break;
                case UITextEnum.FindMemoryCards:
                    showText = text;
                    missionText.text = text;
                    break;
            }
        }

        // private void OnEnable()
        // {
        //     ShowTextAgain();
        // }
        //
        // private void ShowTextAgain()
        // {
        //     missionText.text = showText;
        // }
    }
}