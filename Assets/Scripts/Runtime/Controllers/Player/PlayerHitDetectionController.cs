using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerHitDetectionController : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private Camera camera;

        #endregion


        #region Self Variables

        

        #endregion

        #endregion
        public float maxDistance = 100f; // Maximum distance for raycasting

        private void FixedUpdate()
        {
            // Calculate direction from enemy to player
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.LogWarning(hit.collider.name);
            }
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);
        }

        public void GetCameraTransform(Camera cameraTrans)
        {
            camera = cameraTrans;
        }
    }
}