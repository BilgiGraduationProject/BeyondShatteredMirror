using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Controllers.UI;
using Runtime.Data.UnityObject;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Managers
{
    public class UITextManager : MonoBehaviour
    {
        #region Self Variables

        #region Serilized Variables

         [SerializeField] private MissionTextController missionTextController;
         [SerializeField] private Slider _speedSlider;
         

        #endregion


        #region Private Variables


        private CD_UIMissionTextData _missionData;

        #endregion

        #endregion


        private void Awake()
        {
            _missionData = GetMissionData();
            SendMissionDataToListener(_missionData);
        }

        private void SendMissionDataToListener(CD_UIMissionTextData missionData)
        {
            missionTextController.SetMissionData(missionData);
        }

        private CD_UIMissionTextData GetMissionData() => Resources.Load<CD_UIMissionTextData>("Data/CD_UIMissionText");


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UITextSignals.Instance.onChangeMissionText += missionTextController.OnChangeMissionText;
            missionTextController.OnChangeMissionText();
            PlayerSignals.Instance.onSendPlayerSpeedToSlider += SetSpeedSliderValue;
           
        }

        private void SetSpeedSliderValue(float arg0)
        {
            _speedSlider.value = arg0;
        }


        private void UnSubscribeEvents()
        {
            UITextSignals.Instance.onChangeMissionText -= missionTextController.OnChangeMissionText;
            PlayerSignals.Instance.onSendPlayerSpeedToSlider -= SetSpeedSliderValue;
           
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}