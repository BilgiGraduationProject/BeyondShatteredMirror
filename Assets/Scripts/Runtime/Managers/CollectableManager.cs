using System;
using System.Collections.Generic;
using Runtime.Commands.Collectable;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.Collectable;
using Runtime.Enums.Pool;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class CollectableManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Transform collectableHolder;
        
        

        #endregion

        #region Private Variables

        private CD_Collectable _collectableData;
        private GameObject _emptyObject;
        private CollectableCreateHolderCommand _collectableCreateHolderCommand;
        private readonly string _pathOfData = "Data/CD_Collectable";
        

        

        #endregion

        #endregion

        private void Awake()
        {
            _collectableData = GetCollectableData();
            Init();
            _collectableCreateHolderCommand.Execute();
        }

        private void Init()
        {
            _collectableCreateHolderCommand =
                new CollectableCreateHolderCommand(ref collectableHolder, ref _emptyObject, _collectableData);
        }
        
        private CD_Collectable GetCollectableData() => Resources.Load<CD_Collectable>(_pathOfData);
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EnemySignals.Instance.onEnemyDied += OnEnemyDied;
        }

        private void OnEnemyDied(Transform enemyTransform)
        {
            for (int i = 0; i < _collectableData.Data[(int)CollectableEnum.Soul].CollectableDropAmount; i++)
            {
                var soul = PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Soul);
                soul.transform.parent = collectableHolder.GetChild((int)CollectableEnum.Soul);
            }
            
        }


        private void UnSubscribeEvents()
        {
            EnemySignals.Instance.onEnemyDied -= OnEnemyDied;
        }


        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }

    [System.Serializable]
    public class Collectible
    {
        private float colls;

    }
}