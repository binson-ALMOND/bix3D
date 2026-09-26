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
    [Header("Komponenty do wyłączenia po śmierci")] 
    [SerializeField] private Animator animator;
    [SerializeField] private BossAI bossAI;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Collider mainCollider;
    [Header("Muzyka i Walka")]
    private bool isEncounterStarted = false;
    void Start()
    {
        if(bossCanvas != null) bossCanvas.gameObject.SetActive(false);
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
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

        
        if (bossCanvas != null)
        {
            Destroy(bossCanvas.gameObject);
        }
        MusicManager.Instance.SwitchToExplorationMusic();
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
