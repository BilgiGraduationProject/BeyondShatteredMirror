using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerHitDetectionController : MonoBehaviour
    {
        public Transform player; // Reference to the player GameObject
        public float maxDistance = 100f; // Maximum distance for raycasting

        private void FixedUpdate()
        {
            // Calculate direction from enemy to player
            Vector3 direction = player.position - transform.position;

            // Raycast from enemy towards player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if(hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("Player hit");
                }
            }
            Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
        }
    }
}