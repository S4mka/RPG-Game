using UnityEngine;

public class DamageSword : MonoBehaviour
{
    public int damageAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<MeleeEnemyAI>().TakeDamage(damageAmount);
            
        }
        if (other.tag == "EnemyDal")
        {
            
            other.GetComponent<RangedEnemyAI>().TakeDamage(damageAmount);
        }
    }
}
