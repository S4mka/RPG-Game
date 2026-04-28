using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    public Slider healthbar;
    public bool isDeath = false;
    public GameUI ui;

    // Флаг для блокировки движения при получении урона
    public bool isHit { get; private set; }
    public float hitDuration = 0.5f; // Время стана
    private float hitTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = currentHealth;
    }

    void Update()
    {
        healthbar.value = currentHealth;

        // Обновляем таймер стана
        if (isHit)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0f)
            {
                isHit = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isHit) return; // Не получаем урон повторно во время стана

        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0 && isDeath == false)
        {
            animator.SetTrigger("death");
            GetComponent<Collider>().enabled = false;
            healthbar.gameObject.SetActive(false);
            ui.Over();
            isDeath = true;
            gameObject.SetActive(false);

        }
        else
        {
            animator.SetTrigger("hit");
            HitStun();
        }
    }

    
    

    void HitStun()
    {
        isHit = true;
        hitTimer = hitDuration;
    }
}