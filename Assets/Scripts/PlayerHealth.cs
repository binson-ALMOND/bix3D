using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Punkty Życia")]
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;
    [Header("UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private float smoothSpeed = 8f;
    [Header("I-Frames")] 
    [SerializeField] private float invincibilityDuration = 0.6f;
    private float lastDamageTime = -999f;
    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }
    void Update()
    {
        if (healthBar != null && healthBar.value != currentHealth)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, currentHealth, Time.deltaTime * smoothSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        if (Time.time < lastDamageTime + invincibilityDuration) return;
        
        lastDamageTime = Time.time;
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Gracz zginął!");
    }
}
