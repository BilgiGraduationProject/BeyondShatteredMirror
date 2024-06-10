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
            PlayerHappinessController controller = FindFirstObjectByType<PlayerHappinessController>();
            if (controller != null)
            {
                PlayerSignals.Instance.onSetHappinessValue?.Invoke(happiness);
                Deactivate();
            }
        }
    }
}