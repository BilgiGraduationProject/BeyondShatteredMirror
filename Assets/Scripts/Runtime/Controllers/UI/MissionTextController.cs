using System;
using Runtime.Data.UnityObject;
using Runtime.Enums.Playable;
using Runtime.Enums.UI;
using Runtime.Signals;
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

        private PlayableEnum playabelEnum;
        #endregion

        #region Serialzied Variables

        [SerializeField] private TextMeshProUGUI missionText;

        #endregion

        #endregion
        public void SetMissionData(CD_UIMissionTextData missionData)
        {
            _missionData = missionData;
        }
        
        public void OnChangeMissionText()
        {
            switch (CoreGameSignals.Instance.onSendCurrentGameStateToUIText?.Invoke())
            {
                case PlayableEnum.BathroomLayingSeize:
                    Debug.LogWarning("Text Changed To BathroomLayingSeize");
                    var text =_missionData.data[(int)UITextEnum.GoToMirror].text;
                    missionText.text = text;
                    break;
                case PlayableEnum.EnteredFactory:
                    var text1 =_missionData.data[(int)UITextEnum.FindMemoryCards].text;
                    missionText.text = text1;
                    break;
                case PlayableEnum.EnteredHouse:
                    var text2 =_missionData.data[(int)UITextEnum.LookAtCatEyes].text;
                    missionText.text = $"{text2} {PuzzleSignals.Instance.onGetPuzzleCatEyeValues?.Invoke()}/2";
                    break;
                case PlayableEnum.SecretWall:
                    var text3 =_missionData.data[(int)UITextEnum.TakeBook].text;
                    missionText.text = text3;
                    break;
                case PlayableEnum.DetectiveBoard:
                    var text4 =_missionData.data[(int)UITextEnum.LookAtTheDetectiveBoard].text;
                    missionText.text = text4;
                    break;
                case PlayableEnum.Mansion:
                    var text5 =_missionData.data[(int)UITextEnum.FindLanterns].text;
                    missionText.text = text5;
                    break;
            }
        }

       
    }
}