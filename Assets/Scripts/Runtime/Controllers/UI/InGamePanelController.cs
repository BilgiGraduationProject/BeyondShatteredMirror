using System.Collections.Generic;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signals;
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
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Gradient healthGradient;
        [SerializeField] private Slider happinessBar;
        [SerializeField] private Gradient happinessGradient;
        [SerializeField] private GameObject healthInfo;
        
        private float _currentHealth = 100;
        private float _currentHappiness = 0;
        
        [Space(10)]
        [Header("Tutorial Part")]
        [SerializeField] private GameObject tutorialPart;
        [SerializeField] private Image tutorialImage;
        [SerializeField] private Image tutorialProgress;
        [SerializeField] private Sprite tutorialProgressNotDone;
        [SerializeField] private Sprite tutorialProgressDone;
        public List<TutorialTask> tutorialTasks;
        private TutorialTask currentTask;
        private int currentTaskIndex = 0;

        private void OnEnable()
        {
            SubscribeEvents();
            
            OnHealthPillUsed(0);
            TaskUpdate();
            GetHealthAndHappiness();
        }
        
        void SubscribeEvents()
        {
            CoreUISignals.Instance.onSetHealthSlider += UpdateHealthBar;
            CoreUISignals.Instance.onSetHappinesSlider += UpdateHappinessBar;
        }
        
        void UnsubscribeEvents()
        {
            CoreUISignals.Instance.onSetHealthSlider -= UpdateHealthBar;
            CoreUISignals.Instance.onSetHappinesSlider -= UpdateHappinessBar;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        void GetHealthAndHappiness()
        {
            healthBar.DOValue(_currentHealth / 100f, 0.5f);
            healthBar.value = _currentHealth / 100f;
            healthBar.fillRect.GetComponent<Image>().color = healthGradient.Evaluate(healthBar.normalizedValue);
            happinessBar.DOValue(_currentHappiness / 100f, 0.5f);
            happinessBar.value = _currentHappiness / 100f;
            happinessBar.fillRect.GetComponent<Image>().color = happinessGradient.Evaluate(happinessBar.normalizedValue);
            
            if (_currentHappiness >= 100)
            {
                // TODO: Pill kullan diye güzel bir uyarı vermek lazım
                //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Inventory, 1);
                InputSignals.Instance.onIsReadyForCombat?.Invoke(false);
                
                print("You need to use");
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(healthInfo.transform.DOScale(1.2f, 0.5f));
                mySequence.Append(healthInfo.transform.DOScale(0.9f, 0.5f));
                mySequence.SetLoops(3);
                mySequence.Append(healthInfo.transform.DOScale(1f, 0.5f));
                
                print("You need to use Happiness Pill");
                return;
            }
        }
        
        void UpdateHealthBar(float health)
        {
            _currentHealth = health;
            healthBar.DOValue(health / 100f, 0.5f);
            healthBar.value = health / 100f;
            healthBar.fillRect.GetComponent<Image>().color = healthGradient.Evaluate(healthBar.normalizedValue);
        }
        
        void UpdateHappinessBar(float happiness)
        {
            _currentHappiness += happiness;
            happinessBar.DOValue(_currentHappiness / 100f, 0.5f);
            happinessBar.value = _currentHappiness / 100f;
            happinessBar.fillRect.GetComponent<Image>().color = happinessGradient.Evaluate(happinessBar.normalizedValue);
            
            if (_currentHappiness >= 100)
            {
                // TODO: Pill kullan diye güzel bir uyarı vermek lazım
                //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Inventory, 1);
                InputSignals.Instance.onIsReadyForCombat?.Invoke(false);
                
                print("You need to use");
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(healthInfo.transform.DOScale(1.2f, 0.5f));
                mySequence.Append(healthInfo.transform.DOScale(0.9f, 0.5f));
                mySequence.SetLoops(3);
                mySequence.Append(healthInfo.transform.DOScale(1f, 0.5f));
                
                print("You need to use Happiness Pill");
                return;
            }
        }

        private void TaskUpdate()
        {
            currentTaskIndex = GameDataManager.LoadData<int>("TutorialTask", 0);
            print(currentTaskIndex);
            if (currentTaskIndex >= tutorialTasks.Count)
            {
                AllTasksCompleted(true);
                return;
            }
            currentTask = tutorialTasks[currentTaskIndex];
            tutorialImage.sprite = currentTask.tutorialSprite;
            tutorialProgress.sprite = tutorialProgressNotDone;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                OnHealthPillUsed(1);    
            }
            
            if (currentTask is not null)
            {
                if(currentTask.keyCodes.Count == 0) return;
                
                currentTask.CheckIfAnyKeysPressed();
                if (currentTask.isCompleted && !currentTask.isTaskCompletedTriggered)
                {
                    currentTask.isTaskCompletedTriggered = true;
                    CompleteCurrentTask();
                }
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
                tutorialImage.DOFade(.01f, 1.5f);
                tutorialProgress.DOFade(0.01f, 1.5f).OnComplete(() =>
                {
                    tutorialImage.sprite = tutorialTasks[currentTaskIndex].tutorialSprite;
                    tutorialProgress.sprite = tutorialProgressNotDone;
                    tutorialImage.DOFade(1, 1f);
                    tutorialProgress.DOFade(1, 1f).OnComplete((() =>
                            {
                                currentTask = tutorialTasks[currentTaskIndex];
                            }
                    ));
                });
            }
            else
            {
                AllTasksCompleted(false);
            }
        }

        void AllTasksCompleted(bool value)
        {
            currentTask = null;
            if (value)
            {
                tutorialPart.SetActive(false);
                return;
            }
            tutorialImage.DOFade(.01f, 1.5f);
            tutorialProgress.DOFade(.01f, 1.5f);
            tutorialPart.GetComponent<Image>().DOFade(0.01f, 1.5f).OnComplete(() =>
            {
                tutorialPart.SetActive(false);
            });
        }
        
        private void OnHealthPillUsed(int used)
        {
            int healthPill = GameDataManager.LoadData<int>(GameDataEnums.HealthPill.ToString(), 0);
            if(healthPill <= 0) return;

            if (used != 0)
            {
                healthPill -= used;
                _currentHealth = 100;
                UpdateHealthBar(_currentHealth);
                _currentHappiness = 0;
                UpdateHappinessBar(_currentHappiness);
                PlayerSignals.Instance.onSetHealthValue?.Invoke(_currentHealth);
                InputSignals.Instance.onIsReadyForCombat?.Invoke(true);
                GameDataManager.SaveData(GameDataEnums.HealthPill.ToString(), healthPill);
            }
            
            healthText.text = healthPill.ToString();
        }
    }
}