using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WonderState : StateMachineBehaviour
{
    [SerializeField]
    private float _minDistance;

    [SerializeField]
    private Vector3 _minRange;
    [SerializeField]
    private Vector3 _maxRange;

    [SerializeField]
    private NavMeshAgent _agent;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckAgentReference(animator);
        _agent.isStopped = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_agent.isStopped)
        {
            var targetPos = GetRandomPos(_minDistance, animator.transform.position, _minRange, _maxRange);
            _agent.SetDestination(targetPos);
            _agent.isStopped = false;
        }

        if (_agent.remainingDistance < _agent.stoppingDistance)
            _agent.isStopped = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exit Wonder");
    }

    private void CheckAgentReference(Animator anim)
    {
        if (_agent != null) return;

        _agent = anim.GetComponent<NavMeshAgent>();
    }

    private Vector3 GetRandomPos(float minDistance, Vector3 curPos, Vector3 minRange, Vector3 maxRange)
    {
        Vector3 resultPos = curPos;

        do
        {
            var targetX = Random.Range(minRange.x, maxRange.x);
            var targetZ = Random.Range(minRange.z, maxRange.z);

            resultPos = new Vector3(targetX, resultPos.y, targetZ);
        } while (Vector3.Distance(curPos, resultPos) < minDistance);

        return resultPos;
    }
}
