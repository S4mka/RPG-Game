using UnityEngine;
using UnityEngine.AI;

public class EnemyAIi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Transform target;
    NavMeshAgent agent;

    public float lookRadius;
    public Animator anim;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.player.transform;
    }
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < lookRadius)
        {
            anim.SetBool("isRun", true);
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                anim.SetBool("isAttack", true);
                anim.SetBool("isRun", false);
                Debug.Log("OK");
                LookTarget();


            }
            else 
            {
                anim.SetBool("isAttack", false);
                anim.SetBool("isRun", true);
            }
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    void LookTarget()
    {
        Vector3 direction =(target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookRadius);
    }
}
