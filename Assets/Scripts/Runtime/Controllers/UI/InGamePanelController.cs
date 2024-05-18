using System.Collections.Generic;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    [System.Serializable]
    public class TutorialTask
    {
        public Sprite tutorialSprite;
        public List<KeyCode> keyCodes;
        public bool isCompleted;
        public bool isTaskCompletedTriggered = false;
        
        public void CheckIfAnyKeysPressed()
        {
            foreach (var keyCode in keyCodes)
            {
                if (Input.GetKey(keyCode))
                {
                    isCompleted = true;
                    return;
                }
            }
        }
    }
    
    public class InGamePanelController : MonoBehaviour
    {
        [SerializeField] private Gradient healthGradient;
        [SerializeField] private TextMeshProUGUI healthText;
        
        [SerializeField] private GameObject tutorialPart;
        [SerializeField] private Image tutorialImage;
        [SerializeField] private Image tutorialProgress;
        [SerializeField] private Sprite tutorialProgressNotDone;
        [SerializeField] private Sprite tutorialProgressDone;
        
        public List<TutorialTask> tutorialTasks;
        private TutorialTask currentTask;
        private int currentTaskIndex = 0;
        private bool isReady = false;

        private void OnEnable()
        {
            OnHealthPillUsed(0);
            TaskUpdate();
        }

        private void TaskUpdate()
        {
            currentTaskIndex = GameDataManager.LoadData<int>("TutorialTask", 0);
            print(currentTaskIndex);
            if (currentTaskIndex >= tutorialTasks.Count)
            {
                AllTasksCompleted();
                return;
            }
            currentTask = tutorialTasks[currentTaskIndex];
            tutorialImage.sprite = currentTask.tutorialSprite;
            tutorialProgress.sprite = tutorialProgressNotDone;
        }

        private void Update()
        {
            if (currentTask is null) return;
            
            currentTask.CheckIfAnyKeysPressed();
            if (currentTask.isCompleted && !currentTask.isTaskCompletedTriggered)
            {
                currentTask.isTaskCompletedTriggered = true;
                CompleteCurrentTask();
            }
        }

        void CompleteCurrentTask()
        {
            currentTaskIndex++;
            GameDataManager.SaveData<int>("TutorialTask", currentTaskIndex);
            tutorialProgress.sprite = tutorialProgressDone;
            ShowNextTask();
        }

        private void ShowNextTask()
        {
            if (currentTaskIndex < tutorialTasks.Count)
            {
                // Fade out animation
                tutorialImage.DOFade(.01f, 2f);
                tutorialProgress.DOFade(0.01f, 2f).OnComplete(() =>
                {
                    currentTask = tutorialTasks[currentTaskIndex];
                    tutorialImage.sprite = currentTask.tutorialSprite;
                    tutorialProgress.sprite = tutorialProgressNotDone;
                    tutorialImage.DOFade(1, 0.5f);
                    tutorialProgress.DOFade(1, 0.5f);
                });
            }
            else
            {
                AllTasksCompleted();
            }
        }

        void AllTasksCompleted()
        {
            currentTask = null;
            tutorialPart.SetActive(false);
        }
        
        private void OnHealthPillUsed(int used)
        {
            int healthPill = GameDataManager.LoadData<int>(GameDataEnums.HealthPill.ToString(), 0);
            healthPill -= used;
            GameDataManager.SaveData(GameDataEnums.HealthPill.ToString(), healthPill);
            healthText.text = healthPill.ToString();
        }
    }
}