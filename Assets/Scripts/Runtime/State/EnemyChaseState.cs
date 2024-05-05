using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : StateMachineBehaviour
{
    private float _measure = 120f;
    private NavMeshAgent _agent;
    private Transform _player;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetValues(animator);
    }

    void GetValues(Animator animator)
    {
        _agent = animator.GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player is null)
        {
            GetValues(animator);
            return;
        }
        
        // Check for collisions with other agents
        // Collider[] hitColliders = Physics.OverlapSphere(animator.transform.position, 1.0f);
        // foreach (var hitCollider in hitColliders)
        // {
        //     NavMeshAgent hitAgent = hitCollider.GetComponent<NavMeshAgent>();
        //     if (hitAgent != null && hitAgent != _agent)
        //     {
        //         // If the agent is too close to another agent, update its destination
        //         _agent.SetDestination(_player.position + Vector3.one);
        //         return;
        //     }
        // }
        
        if (_agent.isActiveAndEnabled && NavMesh.SamplePosition(_player.position, out var hit, 1.0f, NavMesh.AllAreas))
        {
            _agent.SetDestination(_player.position);
        }
        
        //_agent.SetDestination(_player.position);
        
        float distance = Vector3.Distance(_player.position, animator.transform.position);
        
        if (distance > _measure)
        {
            animator.SetBool("isChasing", false);
        }
        
        if(distance < 1.25f)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player is null)
        {
            GetValues(animator);
            return;
        }
        
        _agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
