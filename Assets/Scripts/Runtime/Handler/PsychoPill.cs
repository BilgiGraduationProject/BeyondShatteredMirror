using Runtime.Controllers.Player;
using StarterAssets;

namespace Runtime.Handler
{
    public class PsychoPill : BaseSkill
    {
        public float damageMultiplier = 2f;

        public override void Activate()
        {
            base.Activate();
            print("SpeedBoost Activate called");
            PlayerAnimationController controller = FindFirstObjectByType<PlayerAnimationController>();
            if (controller is not null)
            {
                controller.DamageAmountMain *= damageMultiplier;
            }
        }

        public override void Deactivate()
        {
            PlayerAnimationController controller = FindFirstObjectByType<PlayerAnimationController>();
            if (controller is not null)
            {
                controller.DamageAmountMain /= damageMultiplier;
            }
            base.Deactivate();
        }
    }
}