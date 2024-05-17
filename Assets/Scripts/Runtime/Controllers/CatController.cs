using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Controllers
{
    public class CatController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public NavMeshAgent agent;
        public Animator animator;
        public float range;
        public float rotationSpeed;

        public Transform centrePoint;

        #endregion

        #region Serialized Variables

        //

        #endregion

        #region Private Variables

        //

        #endregion

        #endregion

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (agent.remainingDistance <= agent.stoppingDistance) // Yol tamamlandı
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }

            // Dönüş yönünü ayarla
            if (isTurning)
            {
                Vector3 direction = (agent.destination - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // Animasyon hızını ayarla
            if (isTurning)
            {
                animator.speed = 0.5f; // Dönüş sırasında animasyon hızını yarıya düşür
            }
            else
            {
                animator.speed = 1.0f; // Normal hızda animasyon oynat
            }
        }

        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }

        private void SetTurning(bool turning)
        {
            isTurning = turning;
        }

        private bool isTurning;

        public bool IsTurning
        {
            get { return isTurning; }
            set { SetTurning(value); }
        }
    }
}
