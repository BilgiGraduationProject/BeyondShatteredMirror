using UnityEngine;

namespace Runtime.Controllers.Enemy
{
    public class EnemyMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private ParticleSystem soulParticle;

        #endregion

        #endregion

        public void PlaySoulParticle()
        {
            //soulParticle.Play();
        }
    }
}