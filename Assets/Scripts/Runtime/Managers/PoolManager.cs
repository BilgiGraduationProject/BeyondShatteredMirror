using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Commands.Pool;
using Runtime.Data.UnityObject;
using Runtime.Enums.Pool;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class PoolManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject levelHolder;
        [SerializeField] private Transform poolHolder;

        #endregion
        #region Private Variables

        private CD_Pool _poolData;
        private GameObject _emptyObject;
        private PoolGenerateCommand _poolGenerateCommand;
        private PoolResetCommand _poolResetCommand;
        private List<GameObject> _emptyList = new List<GameObject>();
        private readonly string _poolDataPath = "Data/CD_Pool";


        #endregion

        #endregion


        private void Awake()
        {
            _poolData = GetPoolData();
            Init();
            GeneratePool();
        }

        private void Init()
        {
            _poolGenerateCommand = new PoolGenerateCommand(ref _poolData, ref poolHolder, ref _emptyObject);
            _poolResetCommand = new PoolResetCommand(ref _poolData, ref poolHolder, ref levelHolder);
        }
        private void GeneratePool()
        {
            _poolGenerateCommand.Execute();

        }
        private CD_Pool GetPoolData() => Resources.Load<CD_Pool>(_poolDataPath);

        private async void RestartPool()
        {
            await Task.Delay(500);
            _poolResetCommand.Execute();
        }


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PoolSignals.Instance.onSendPool += OnSendPool;
            PoolSignals.Instance.onGetPoolObject += OnGetPoolObject;
        }

        private void OnSendPool(GameObject poolObj, PoolType poolType)
        {
            poolObj.transform.parent = poolHolder.GetChild((int)poolType);
            poolObj.transform.localPosition = Vector3.zero;
        }

        private GameObject OnGetPoolObject(PoolType poolType)
        {
            var parent = poolHolder.transform.GetChild((int)poolType);

            if (parent.childCount > 0)
            {
                var obj = parent.transform.GetChild(0).gameObject;
                return obj;
            }
            else
            {
                var obj = Instantiate(_poolData.Data[(int)poolType].ObjPrefab, Vector3.zero,Quaternion.identity, poolHolder.GetChild((int)poolType));
                return obj;
            }

        }

        private void UnSubscribeEvents()
        {
            PoolSignals.Instance.onSendPool -= OnSendPool;
            PoolSignals.Instance.onGetPoolObject -= OnGetPoolObject;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}