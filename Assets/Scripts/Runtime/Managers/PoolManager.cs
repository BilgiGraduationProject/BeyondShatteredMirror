using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Runtime.Commands.Pool;
using Runtime.Data.UnityObject;
using Runtime.Enums.GameManager;
using Runtime.Enums.Playable;
using Runtime.Enums.Pool;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Managers
{
    public class PoolManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject levelHolder;
        [SerializeField] private Transform poolHolder;
        [SerializeField] private Transform houseHolder;
        [SerializeField] private Transform fightHolder;
        #endregion
        
        #region Private Variables

        private CD_Pool _poolData;
        private GameObject _emptyObject;
        private PoolGenerateCommand _poolGenerateCommand;
        private PoolResetCommand _poolResetCommand;
        private List<GameObject> _emptyList = new List<GameObject>();
        private readonly string _poolDataPath = "Data/CD_Pool";
        
        private PlayableEnum _currentPlayableEnum;
        
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
            PoolSignals.Instance.onGetPoolObject += OnGetPoolObject;
            PoolSignals.Instance.onSendPool += OnSendPool;
            PoolSignals.Instance.onGetLevelHolderTransform += OnGetLeveLHolderTransform;
            PoolSignals.Instance.onGetLevelHolderPoolObject += OnGetLevelHolderPoolObject;
            PoolSignals.Instance.onLoadLevel += OnLoadLevel;
            PoolSignals.Instance.onDestroyTheCurrentLevel += OnDestroyTheCurrentLevel;
            PoolSignals.Instance.onSetAslanHouseVisible += OnSetAslanHouseVisible;
            PoolSignals.Instance.onSetCurrentLevelToVisible += OnSetCurrentLevelToVisible;
            PoolSignals.Instance.onDestroyFightLevel += OnDestroyFightLevel;
        }

        private void OnDestroyFightLevel()
        {
            PlayerSignals.Instance.onPlayerLoadTransform?.Invoke();
            Destroy(fightHolder.transform.GetChild(0).gameObject);
            
        }

        private void OnSetCurrentLevelToVisible(bool condition)
        {
           
            levelHolder.transform.GetChild(0).gameObject.SetActive(condition);
           
            
            
            
           
        }

        private void OnSetAslanHouseVisible(bool condition)
        {
            houseHolder.transform.GetChild(0).gameObject.SetActive(!condition);
        }

        private void OnDestroyTheCurrentLevel()
        {
            if (levelHolder.transform.childCount is 0) return;
            for (int i = 0; i < levelHolder.transform.childCount; i++)
            {
                var destroyLevel = levelHolder.transform.GetChild(i).gameObject;
                Destroy(destroyLevel);
            }
            
        }


        [Button("Load Level")]
        private void OnLoadLevel(LevelEnum levelEnum,PlayableEnum playableEnum)
        {
            _currentPlayableEnum = playableEnum;
            AsyncOperationHandle<GameObject> asyncOperationHandle =
                Addressables.LoadAssetAsync<GameObject>($"Assets/Levels/{levelEnum.ToString()}.prefab");
            
                asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
        }
        
        
        private void AsyncOperationHandle_Completed(AsyncOperationHandle<GameObject> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                switch (_currentPlayableEnum)
                {
                    case PlayableEnum.BathroomLayingSeize:
                        Instantiate(obj.Result, houseHolder.transform);
                        PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(_currentPlayableEnum);
                        break;
                    
                    case PlayableEnum.EnteredFactory:
                        Debug.LogWarning("Entered Factory instaisted");
                        Instantiate(obj.Result, levelHolder.transform);
                        PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(_currentPlayableEnum);
                        CoreUISignals.Instance.onCloseUnCutScene?.Invoke(_currentPlayableEnum);
                        break;
                    case PlayableEnum.Mansion:
                        Instantiate(obj.Result, levelHolder.transform);
                        PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(_currentPlayableEnum);
                        break;
                    case PlayableEnum.SpawnPoint:
                        Instantiate(obj.Result, fightHolder.transform);
                        PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(_currentPlayableEnum);
                        DOVirtual.DelayedCall(1.5f, () =>
                    {
                        CoreUISignals.Instance.onCloseUnCutScene?.Invoke(_currentPlayableEnum);
                    });
                        
                       
                        break;
                    
                }
                
            }
            else
            {
                Debug.LogWarning("Failed to Load Level");
            }
        }

        private Transform OnGetLeveLHolderTransform() => levelHolder.transform;
        

        private GameObject OnGetPoolObject(PoolType poolType, Transform target)
        {
            var parent = poolHolder.transform.GetChild((int)poolType);

            if (parent.childCount > 0)
            {
                var obj = parent.transform.GetChild(0).gameObject;
                obj.transform.parent = target;
                obj.transform.position = target.position;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                var obj = Instantiate(_poolData.Data[(int)poolType].ObjPrefab, target.position,Quaternion.identity, poolHolder.GetChild((int)poolType));
                return obj;
            }
        }


        private GameObject OnGetLevelHolderPoolObject(PoolType poolType, Transform target)
        {
            var parent = poolHolder.transform.GetChild((int)poolType);
            if (target.childCount > 0)
            {
                var obj = target.transform.GetChild(0).gameObject;
                Destroy(obj);
                var newObj = parent.transform.GetChild(0).gameObject;
                newObj.transform.parent = target;
                newObj.SetActive(true);
                return newObj;

            }

            else
            {
                var obj = parent.transform.GetChild(0).gameObject;
                obj.transform.parent = target;
                obj.SetActive(true);
                return obj;
            }
        }

        private void OnSendPool(GameObject poolObj, PoolType poolType)
        {
            poolObj.SetActive(false);
            poolObj.transform.parent = poolHolder.GetChild((int)poolType);
            poolObj.transform.localPosition = Vector3.zero;
        }
        
        private void UnSubscribeEvents()
        {
            PoolSignals.Instance.onGetPoolObject -= OnGetPoolObject;
            PoolSignals.Instance.onSendPool -= OnSendPool;
            PoolSignals.Instance.onGetLevelHolderTransform -= OnGetLeveLHolderTransform;
            PoolSignals.Instance.onGetLevelHolderPoolObject -= OnGetLevelHolderPoolObject;
            PoolSignals.Instance.onLoadLevel -= OnLoadLevel;
            PoolSignals.Instance.onDestroyTheCurrentLevel -= OnDestroyTheCurrentLevel;
            PoolSignals.Instance.onSetAslanHouseVisible -= OnSetAslanHouseVisible;
            PoolSignals.Instance.onSetCurrentLevelToVisible -= OnSetCurrentLevelToVisible;
            PoolSignals.Instance.onDestroyFightLevel -= OnDestroyFightLevel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}