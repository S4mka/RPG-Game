using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;
    public Image fillImage;

    private void Start()
    {
        health.OnHealthChanged += UpdateBar;
    }

    void UpdateBar(float currentHP)
    {
        fillImage.fillAmount = currentHP / 100f;
    }
}