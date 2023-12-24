using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Controllers
{
    public class SoulController : MonoBehaviour, ICollectable
    {
        public void OnCollect()
        {
            Debug.Log("Soul collected");
            Destroy(gameObject.GetComponent<CapsuleCollider>());
            Destroy(gameObject, .1f);
        }

        public void OnBreak()
        {
            Debug.Log("Soul break");
        }
    }
}