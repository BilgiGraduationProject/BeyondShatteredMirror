using Runtime.Controllers.Player;
using Runtime.Signals;

namespace Runtime.Handler
{
    public class HealthPill : BaseSkill
    {
        public float health = 100f;

        public override void Activate()
        {
            base.Activate();
            PlayerHealthController controller = FindFirstObjectByType<PlayerHealthController>();
            if (controller != null)
            {
                PlayerSignals.Instance.onSetHealthValue?.Invoke(health);
                Deactivate();
            }
        }
    }
}