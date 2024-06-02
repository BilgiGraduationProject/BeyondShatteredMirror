using StarterAssets;
using UnityEngine;

namespace Runtime.Handler
{
    public class SonicPerkPill : BaseSkill
    {
        public float speedMultiplier = 1.8f;
        public float sprintMultiplier = 1.8f;

        public override void Activate()
        {
            base.Activate();
            print("SpeedBoost Activate called");
            ThirdPersonController controller = FindFirstObjectByType<ThirdPersonController>();
            if (controller is not null)
            {
                controller.MoveSpeed *= speedMultiplier;
                controller.SprintSpeed *= sprintMultiplier;
            }
        }

        public override void Deactivate()
        {
            ThirdPersonController controller = FindFirstObjectByType<ThirdPersonController>();
            if (controller is not null)
            {
                controller.MoveSpeed /= speedMultiplier;
                controller.SprintSpeed /= sprintMultiplier;
            }
            base.Deactivate();
        }
    }
}
