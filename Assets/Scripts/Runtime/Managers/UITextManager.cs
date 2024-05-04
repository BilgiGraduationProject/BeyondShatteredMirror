using System;
using Runtime.Controllers.UI;
using Runtime.Data.UnityObject;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Managers
{
    public class UITextManager : MonoBehaviour
    {
        #region Self Variables

        #region Serilized Variables

         [SerializeField] private MissionTextController missionTextController;

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
        }

        private void UnSubscribeEvents()
        {
            UITextSignals.Instance.onChangeMissionText -= missionTextController.OnChangeMissionText;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}