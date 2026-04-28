using UnityEngine;
using UnityEngine.InputSystem;

public class MagicSpell : MonoBehaviour
{
    Animator animator;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;

    private float nextFireTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);

    }

    // Update is called once per frame
    void Update()
    {
        bool isRightMousePressed = Mouse.current.rightButton.isPressed;
        if (isRightMousePressed && Time.time >= nextFireTime) 
        {
            
            Shoot();
            nextFireTime = Time.time + fireRate;
            
        }
        void Shoot()
        {
            Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
            
        }
    }
}
