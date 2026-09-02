using Mono.Cecil;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Parametry Ataku")]
    [SerializeField] private float swordDamage = 25f;
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Animacja")] 
    [SerializeField] private Animator animator;

    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private float lastAttackTime = -999f;
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    
    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger(AttackHash);
        }
    }
    public void OnAttackHit(){
        Vector3 attackCenter = transform.position + (transform.forward * attackDistance) + (Vector3.up*0.8f);
        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter, attackRadius, enemyLayers);
        foreach (Collider hit in hitEnemies)
        {
            IDamageable target = hit.GetComponent<IDamageable>() ?? hit.GetComponentInParent<IDamageable>();
            if (target != null && hit.gameObject != gameObject)
            {
                target.TakeDamage(swordDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackCenter = transform.position + (transform.forward * attackDistance) + (Vector3.up * 0.8f);
        Gizmos.DrawWireSphere(attackCenter,attackRadius);
    } 
}
