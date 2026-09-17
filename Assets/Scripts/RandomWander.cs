using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;
using Random = UnityEngine.Random;
[RequireComponent(typeof(NavMeshAgent))]
public class RandomWander : MonoBehaviour
{
    [SerializeField] private float wanderRadius = 8f;
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 6f;
    private NavMeshAgent agent;
    private float timer;
    private float currentWaitTime;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
        MoveToRandomPoint();
    }
    
    void Update()
    {
        if (!agent.isOnNavMesh) return;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            timer += Time.deltaTime;

            if (timer >= currentWaitTime)
            {
                MoveToRandomPoint();
                timer = 0f;
                currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }

    private void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
