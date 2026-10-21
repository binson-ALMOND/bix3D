using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BossHealth : MonoBehaviour, IDamageable
{
    [Header("HP")] 
    [SerializeField] private float maxHealth = 40f;
    private float currentHealth;
    private bool isDead = false;

    [Header("UI")] 
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Canvas bossCanvas;
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private TextMeshProUGUI healthText;
    [Header("Komponenty do wyłączenia po śmierci")] 
    [SerializeField] private Animator animator;
    [SerializeField] private BossAI bossAI;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Collider arenaCollider;
    [Header("Muzyka i Walka")]
    public bool isEncounterStarted = false;
    private HitFlash hitFlash;

    void Awake()
    {
        hitFlash = GetComponent<HitFlash>();
    }
    void Start()
    {
        if(bossCanvas != null) bossCanvas.gameObject.SetActive(false);
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
        if(animator == null) animator = GetComponent<Animator>();
        if (bossAI == null) bossAI = GetComponent<BossAI>();
        if (mainCollider == null) mainCollider = GetComponent<Collider>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }

        if (hitFlash != null)
        {
            hitFlash.Flash();
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        
        if(bossAI != null) bossAI.enabled = false;
        if (agent != null) agent.isStopped = true;
        if(agent != null) agent.enabled = false;
        if (mainCollider != null) mainCollider.enabled = false;
        if (animator != null) animator.SetTrigger("Die");
        if(arenaCollider != null) arenaCollider.enabled = false;
        
        if (bossCanvas != null)
        {
            Destroy(bossCanvas.gameObject);
        }

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.TriggerVictory();
        }

        if (gameOverManager != null)
        {
            gameOverManager.TriggerVictory();
        }
        
    }

    public void StartEncounter()
    {
        if (isEncounterStarted || isDead) return;
        if(bossCanvas != null) bossCanvas.gameObject.SetActive(true);
        if(healthSlider != null) healthSlider.gameObject.SetActive(true);
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SwitchToBossMusic();
        }
    }
}
