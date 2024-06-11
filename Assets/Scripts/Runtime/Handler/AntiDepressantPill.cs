using Runtime.Controllers.Player;
using Runtime.Signals;

namespace Runtime.Handler
{
    public class AntiDepressantPill : BaseSkill
    {
        public float happiness = 100f;

        public override void Activate()
        {
            base.Activate();
            var controller = FindFirstObjectByType<PlayerHappinessController>();
            if (controller == null) return;
            
            PlayerSignals.Instance.onSetHappinessValue?.Invoke(happiness);
            Deactivate();
        }
    }
}