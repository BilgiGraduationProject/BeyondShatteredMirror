using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Runtime.Controllers.Enemy
{
    public class WanderingEnemyDetectionController : MonoBehaviour
    {
        #region Self Variables



        #region Private Variables

        private Camera _camera;
        private NativeArray<RaycastCommand> _raycastCommands;
        private NativeArray<RaycastHit> _raycastHits;


        private JobHandle _jobHandle;
        private GameObject _collidedObject;
        private bool _didHit;
        private GameObject enemyObj;

        #endregion

        #endregion

        [SerializeField] private float maxDistance; // Maximum distance for raycasting
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerHandTransform;
        [SerializeField] private Transform playerKillTransform;


        private void Awake()
        {
            _raycastCommands = new NativeArray<RaycastCommand>(1, Allocator.Persistent);
            _raycastHits = new NativeArray<RaycastHit>(1, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            _jobHandle.Complete();
            _raycastCommands.Dispose();
            _raycastHits.Dispose();
        }


        private void FixedUpdate()
        {


            _jobHandle.Complete();
            RaycastHit pickUpRay = _raycastHits[0];
            bool pickUpDidHitYa = pickUpRay.collider is not null;
            CheckForPickUpRay(pickUpDidHitYa, pickUpRay);

            var newRay = transform.forward;
            _raycastCommands[0] = new RaycastCommand(transform.position, newRay, maxDistance, LayerMask.GetMask("Player","Grass"));
            Debug.DrawRay(transform.position, newRay * maxDistance, Color.cyan);
            _jobHandle = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 1);
        }

        private void CheckForPickUpRay(bool pickUpDidHitYa, RaycastHit pickUpRay)
        {
            if (pickUpDidHitYa)
            {
                
               
                    if(pickUpRay.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.LogWarning("Player Detected");
                        enemyObj = pickUpRay.collider.gameObject;
                       
                    }
                    else if(pickUpRay.collider.gameObject.CompareTag("Grass"))
                    {
                        Debug.LogWarning("Grass Detected");
                        enemyObj = pickUpRay.collider.gameObject;
                        
                    }
                    else
                    {
                        Debug.LogWarning("Unknown Object Detected");
                   
                 
                    }
                

            }
           



            }
        }
    
}