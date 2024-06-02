using Runtime.Controllers.Player;
using StarterAssets;

namespace Runtime.Handler
{
    public class ShieldPill : BaseSkill
    {
        public float shieldMultiplier = 0.6f;

        public override void Activate()
        {
            base.Activate();
            print("SpeedBoost Activate called");
            PlayerHealthController controller = FindFirstObjectByType<PlayerHealthController>();
            if (controller is not null)
            {
                controller.DamageDealthAmount *= shieldMultiplier;
            }
        }

        public override void Deactivate()
        {
            PlayerHealthController controller = FindFirstObjectByType<PlayerHealthController>();
            if (controller is not null)
            {
                controller.DamageDealthAmount /= shieldMultiplier;
            }
            base.Deactivate();
        }
    }
}