using System;
using UnityEngine;

namespace Runtime.Managers
{
    public class DebugManager : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            var newPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            Gizmos.DrawRay(newPos, 2 * transform.forward);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(newPos, 2 * transform.right);
        }

        private void Update()
        {
           
        }
    }
}