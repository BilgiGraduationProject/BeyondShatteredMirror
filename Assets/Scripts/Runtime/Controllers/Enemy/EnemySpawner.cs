using DG.Tweening;
using Runtime.Enums.Pool;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        #region Self Variables

        #region Public  Variables

        public GameObject enemyPrefab;
        public Transform[] spawnPoints = new Transform[5];

        #endregion

        #region Serialized Variables

        //

        #endregion

        #region Private Variables

        private int stage = 1;
        private int totalStages = 5;
        private int enemiesKilled = 0;
        private int enemiesToSpawn;

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }
          
        void SubscribeEvents()
        {
            EnemySignals.Instance.onEnemyDied += EnemyKilled;
        }
          
        void UnSubscribeEvents()
        {
            EnemySignals.Instance.onEnemyDied -= EnemyKilled;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        void Start()
        {
            SpawnEnemies();
        }

        void SpawnEnemies()
        {
            enemiesToSpawn = stage; // Set the number of enemies to spawn based on the current stage

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                // Use the current stage as the index for the spawnPoints array
                //Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
                PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Enemy, spawnPoints[i]);
            }

            print($"Stage {stage} |-_-| {enemiesKilled}/{enemiesToSpawn} enemies killed");
            //uiManager.UpdateUI(stage, enemiesKilled, enemiesToSpawn); // Update the UI
        }

        void EnemyKilled()
        {
            enemiesKilled++;

            if (enemiesKilled >= enemiesToSpawn)
            {
                DOVirtual.DelayedCall(1.5f ,() =>
                {
                    NextStage();
                });
            }
            
            print($"Stage {stage} |-_-| {enemiesKilled}/{enemiesToSpawn} enemies killed");
            //uiManager.UpdateUI(stage, enemiesKilled, enemiesToSpawn); // Update the UI
        }

        void NextStage()
        {
            stage++;
            enemiesKilled = 0;

            if (stage > totalStages)
            {
                Complete();
            }
            else
            {
                SpawnEnemies();
            }
        }

        void Complete()
        {
            //uiManager.ShowCompleteMessage(); // Show the complete message
            print("All stages complete!");
        }
    }
}