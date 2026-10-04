using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Punkty Życia")]
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;
    [Header("UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float smoothSpeed = 8f;
    [Header("I-Frames")] 
    [SerializeField] private float invincibilityDuration = 0.6f;
    [Header("References")]
    [SerializeField] private PlayerAudio playerAudio;
    [SerializeField] private GameOverManager gameOverManager;

    private float lastDamageTime = -999f;
    void Start()
    {
        if(playerAudio == null) playerAudio = GetComponent<PlayerAudio>();
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
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
        if (playerAudio != null)
        {
            playerAudio.PlayHurtSound();
        }
        CameraShake.Instance.Shake(0.3f, 0.12f);
        lastDamageTime = Time.time;
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealToMax()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        CameraShake.Instance.StopShake();
        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
            GetComponent<PlayerMovement>().enabled = false;  
        }
    } 
}
