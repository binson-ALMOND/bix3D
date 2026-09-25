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
    
    void Start()
    {
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
    }
}
