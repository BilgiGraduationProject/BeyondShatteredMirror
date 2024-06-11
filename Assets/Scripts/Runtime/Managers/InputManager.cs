
using System;
using System.Collections;
using DG.Tweening;
using Runtime.Commands.Input;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Enums.UI;
using Runtime.Keys.Input;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables




        [Header("Variables")] 
        [Header("Movement")]
        private bool _isCanTouchButton;
       
        private PlayableEnum _playableEnumIndex;
        [Header(("Cancel Movement"))] 
        private bool _isMovementInputIsReadyToUse = true;
        [Header(("Combat"))]
        private int _combatCount;
        private Coroutine _combatCoroutine;
        private bool _isCombat;
        [Header("Actions")]
        
        private bool _isCrouch;
        private bool _isCutSceneInputReadyToUse;
        private bool _isPickingUp;
        


        [Header("Input Keys")] 
        private readonly string horizontal = "Horizontal";
        private readonly string vertical = "Vertical";
        private readonly string leftControl = "Left Control";
        private readonly string leftShift = "Left Shift";
        private readonly string space = "Space";
        private readonly string leftMouseButton = "LeftMouseButton";


        [Header("Commands")]
        private CrouchInputCommand _crouchInputCommand;
        private SpaceInputCommand _spaceInputCommand;
        private MeeleCombatCommand _meeleCombatCommand;

        

        #endregion

        #endregion


        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _crouchInputCommand = new CrouchInputCommand(leftControl);
            _spaceInputCommand = new SpaceInputCommand(space);
            _meeleCombatCommand = new MeeleCombatCommand(leftMouseButton,this,ref _combatCoroutine);
            
        }
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onChangeMouseVisibility += OnChangeMouseVisibility;
            InputSignals.Instance.onChangeCrouchState += OnChangeCrouchState;
            InputSignals.Instance.onIsReadyForCombat += OnIsReadyForCombat;
            PlayableSignals.Instance.onSendInputManagerToReadyForInput += OnSendInputManagerToReadyForInput;
            InputSignals.Instance.onGetCombatState += () => _isCombat;
            InputSignals.Instance.onIsPlayerReadyToMove += (condition) => _isMovementInputIsReadyToUse = condition;
        }

        private void OnChangeCrouchState(bool condiiton)
        {
            _isCrouch = condiiton;
            InputSignals.Instance.onPlayerPressedLeftControlButton?.Invoke(condiiton);
        }

        private void OnIsReadyForCombat(bool condition) => _isCombat = condition;
        
        
        private void OnIsPlayerPickingItem(bool condition)
        {
            _isPickingUp = condition;
        }
        

        private void OnSendInputManagerToReadyForInput(bool condition, PlayableEnum playableEnum)
        {
            _isCutSceneInputReadyToUse = condition;
            _playableEnumIndex = playableEnum;
        }
        

        private void OnChangeMouseVisibility(bool condition)
        {
            if (condition)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }



        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onChangeMouseVisibility -= OnChangeMouseVisibility;
            InputSignals.Instance.onIsReadyForCombat -= OnIsReadyForCombat;
            InputSignals.Instance.onChangeCrouchState -= OnChangeCrouchState;
            PlayableSignals.Instance.onSendInputManagerToReadyForInput -= OnSendInputManagerToReadyForInput;
            InputSignals.Instance.onGetCombatState -= () => _isCombat;
            InputSignals.Instance.onIsPlayerReadyToMove -= (condition) => _isMovementInputIsReadyToUse = condition;
        }

       

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Update()
        {
            if (_isCutSceneInputReadyToUse)
            {
                if (Input.GetButtonDown(horizontal) || Input.GetButtonDown(vertical))
                {
                    switch (_playableEnumIndex)
                    {
                        case PlayableEnum.BathroomLayingSeize:
                            PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandUp);
                            _isCutSceneInputReadyToUse = false;
                            break;
                        case PlayableEnum.EnteredHouse:
                            PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandUp);
                            _isCutSceneInputReadyToUse = false;
                            break;
                        
                    }
                }
                    
            }

            if (!_isMovementInputIsReadyToUse) return;
            if (Input.GetKeyDown(KeyCode.E) )
            {
                Debug.LogWarning("Pressing Pick up");
                InputSignals.Instance.onPlayerPressedPickUpButton?.Invoke();
            }
            _crouchInputCommand.Execute(ref _isCrouch,_isMovementInputIsReadyToUse);
            _spaceInputCommand.Execute(_isCrouch,_isMovementInputIsReadyToUse);
            // -------------------------------------------------
            
            _meeleCombatCommand.Execute(ref _combatCount,_isCombat,_isMovementInputIsReadyToUse);

            

            if (Input.GetKeyDown(KeyCode.X))
            {
                InputSignals.Instance.onPlayerPressedDropItemButton?.Invoke();
            }
            




        }

        internal IEnumerator CancelPlayerCombat()
        {
            
            yield return new WaitForSeconds(3f);
            _combatCount = 0;
            yield return new WaitForSeconds(10f);
            PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Combat, false);
        }
    }

       
    }


