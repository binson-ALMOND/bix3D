using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Parametry życia")]
    [SerializeField] private float maxHealth = 5f;
    private float currentHealth;
    
    [Header("Pasek HP")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private float smoothSpeed = 8f;

    [Header("Hit Flash")] 
    [SerializeField] private Renderer slimeRenderer;
    [SerializeField] private Color hitColor = Color.red;
    private Color originalColor;
    private Material slimeMaterial;
    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }

        if (slimeRenderer == null)
        {
            slimeRenderer = GetComponentInChildren<Renderer>();
        }

        if (slimeRenderer != null)
        {
            slimeMaterial = slimeRenderer.material;
            originalColor = slimeMaterial.color;
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
        currentHealth -= damage;
        currentHealth = Mathf.Max(0f , currentHealth);

        if (slimeMaterial != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashEffect());
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashEffect()
    {
        slimeMaterial.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        slimeMaterial.color = originalColor;
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
