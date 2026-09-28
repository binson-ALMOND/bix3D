using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    public enum BossState {Idle, Chase, MeleeAttack}
    
    [Header("Zasięgi")]
    [SerializeField] private float chaseRange = 15f;
    [SerializeField] private float meleeRange = 3f;
    [SerializeField] private float minRangedRange = 6.0f;
    [Header("Cooldowny")]
    [SerializeField] private float meleeCooldown = 2.5f;
    [SerializeField] private float slamCooldown = 8f;
    [SerializeField] private float screamCooldown = 12f;
    [Header("Referencje")]
    [SerializeField] private Transform player;

    [Header("Obrażenia w zwarciu")] 
    [SerializeField] private BossWeaponHitbox weaponHitbox;
    [SerializeField] private int lightAttackDamage = 1;
    [SerializeField] private int wideAttackDamage = 2;

    private int currentAttackDamage;
    
    private NavMeshAgent agent;
    private Animator animator;
    
    private BossState currentState = BossState.Idle;
    private bool canRotate = true;
    private bool isExecutingAction = false;
    
    private float lastMeleeTime;
    private float lastSlamTime;
    private float lastScreamTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if(playerObj != null) player = playerObj.transform;
        }
    }

    
    private void Update()
    {
        if (player == null) return;
        if (isExecutingAction)
        {
            agent.isStopped = true;
            if (canRotate)
            {
                RotateTowardsPlayer();
            }
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState(distanceToPlayer);
                if(distanceToPlayer <= chaseRange) currentState = BossState.Chase;
                break;
            case BossState.Chase:
                HandleChaseState(distanceToPlayer);
                break;
            case BossState.MeleeAttack:
                HandleMeleeState(distanceToPlayer);
                break;
        }
        bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;
        animator.SetBool("isMoving", isMoving);
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        agent.isStopped = true;
        if (distanceToPlayer <= chaseRange)
        {
            currentState = BossState.Chase;
        }
    }

    private void HandleChaseState(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        if (distanceToPlayer <= meleeRange)
        {
            currentState = BossState.MeleeAttack;
            return;
        }
        if (distanceToPlayer >= minRangedRange && distanceToPlayer <= chaseRange)
        {
            if (Time.time >= lastSlamTime + slamCooldown)
            {
                TriggerAction("GroundSlam");
                lastSlamTime = Time.time;
                return;
            }

            if (Time.time >= lastScreamTime + screamCooldown)
            {
                TriggerAction("Scream");
                lastScreamTime = Time.time;
                return;
            }
        }

        if (distanceToPlayer > chaseRange * 1.3f)
        {
            currentState = BossState.Idle;
        }
    }

    private void HandleMeleeState(float distanceToPlayer)
    {
        if (distanceToPlayer > meleeRange)
        {
            currentState = BossState.Chase;
            return;
        }
        agent.isStopped = true;
        RotateTowardsPlayer();
        if (Time.time >= lastMeleeTime + meleeCooldown)
        {
            lastMeleeTime = Time.time;

            if (Random.value > 0.5f)
            {
                TriggerAction("AttackLight");
            }
            else
            {
                TriggerAction("AttackWide");
            }
        }
    }

    private void TriggerAction(string action)
    {
        isExecutingAction = true;
        canRotate = true;
        agent.isStopped = true;

        if (action == "AttackLight") currentAttackDamage = lightAttackDamage;
        if (action == "AttackWide") currentAttackDamage = wideAttackDamage;
        
        animator.SetTrigger(action);
    }

    public void EndAction()
    {
        isExecutingAction = false;
        canRotate = true;
        weaponHitbox?.DisableWeaponHitbox();
    }
    
    private void RotateTowardsPlayer()
    {
        if (!canRotate) return;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minRangedRange);
        
    }

    public void LockRotation()
    {
        canRotate = false;
    }

    public void UnlockRotation()
    {
        canRotate = true;
    }

    public void EnableWeaponHitbox()
    {
        weaponHitbox?.EnableWeaponHitbox(currentAttackDamage);
    }

    public void DisableWeaponHitbox()
    {
        weaponHitbox?.DisableWeaponHitbox();
    }
}
