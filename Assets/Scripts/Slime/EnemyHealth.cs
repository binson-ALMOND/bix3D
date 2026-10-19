using System;
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
    private bool isDead = false;
    
    [Header("Efekt Śmierci")]
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float deathDuration = 0.25f;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip takeDamageSound;
    

    [Header("Loot")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinsToDrop;
    private HitFlash hitFlash;


    void Awake()
    {
        hitFlash = GetComponent<HitFlash>();
    }
    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
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
        if(isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Max(0f , currentHealth);

        PlayHitSound();
        if (hitFlash != null)
        {
            hitFlash.Flash();
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }



    private void PlayHitSound()
    {
        if (audioSource != null && takeDamageSound != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.85f, 1.2f);
            audioSource.PlayOneShot(takeDamageSound);
        }
    }
    
    private void Die()
    {
        if(isDead) return;
        isDead = true;
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.RegisterSlimeKill();
        }
        if (deathVFXPrefab != null)
        {
            Instantiate(deathVFXPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        if (coinPrefab != null)
        {
            for (int i = 0; i < coinsToDrop; i++)
            {
                Instantiate(coinPrefab, transform.position + Vector3.up * 0.5f, coinPrefab.transform.rotation);
            }
        }

        Collider col = GetComponent<Collider>();
        if(col != null) col.enabled = false;
        SlimeAI ai = GetComponent<SlimeAI>();
        if(ai != null) ai.enabled = false;

        StartCoroutine(ShrinkAndDestroy());
        
    }

    private IEnumerator ShrinkAndDestroy()
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < 0.2f)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / deathDuration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
