using Runtime.Controllers.Player;

namespace Runtime.Handler
{
    public class PulseOfImmortalityPerkPill : BaseSkill
    {
        public float blockMultiplier = 0f;

        public override void Activate()
        {
            base.Activate();
            print("SpeedBoost Activate called");
            PlayerHealthController controller = FindFirstObjectByType<PlayerHealthController>();
            if (controller is not null)
            {
                controller.DamageDealthAmount *= blockMultiplier;
            }
        }

        public override void Deactivate()
        {
            PlayerHealthController controller = FindFirstObjectByType<PlayerHealthController>();
            if (controller is not null)
            {
                controller.DamageDealthAmount = 1f;
            }
            base.Deactivate();
        }
    }
}