using Runtime.Enums.Player;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Input
{
    public struct MeeleCombatCommand
    {
        private readonly string _leftMouseButton;
        private readonly InputManager _inputManager;
        private Coroutine _combatCoroutine;
        
        public MeeleCombatCommand(string leftMouseButton, InputManager inputManager, ref Coroutine combatCoroutine)
        {
            _leftMouseButton = leftMouseButton;
            _inputManager = inputManager;
            _combatCoroutine = combatCoroutine;
        }


        public void Execute(ref int combatCount, bool isCombat)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (_combatCoroutine != null)
                {
                    _inputManager.StopCoroutine(_combatCoroutine);
                }
                if (!isCombat) return;
                if (combatCount >= 4)
                {
                    combatCount = 0;
                }
                combatCount++;
                PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Attack, true);
                PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Combat, true);
                PlayerSignals.Instance.onSetCombatCount?.Invoke(combatCount);
                Debug.LogWarning(combatCount);

            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _combatCoroutine = _inputManager.StartCoroutine(_inputManager.CancelPlayerCombat());
            }

        }
    }
}