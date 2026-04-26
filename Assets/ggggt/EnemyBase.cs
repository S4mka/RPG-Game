using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;

    public float chaseRadius = 10f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRadius)
            agent.SetDestination(player.position);
    }

    protected abstract void Attack();
}