using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{   
    public enum WeaponType
    {
        Sword,
        Bow
    }

    public WeaponType currentWeapon = WeaponType.Sword;

    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject bow;
    [Header("Parametry Miecza")]
    [SerializeField] private float swordDamage = 25f;
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float swordCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Parametry Łuku")] 
    [SerializeField] private float bowCooldown = 1.0f;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float arrowForce = 35f;
    [SerializeField] private GameObject visualArrow;
    
    [Header("Animacja")] 
    [SerializeField] private Animator animator;
    [SerializeField] private Animator bowAnimator;

    [Header("Dostępność broni")] 
    public bool isBowUnlocked = false;
    
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int BowShootHash = Animator.StringToHash("BowShoot");
    private float lastAttackTime = -999f;
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        EquipSword();
    }

    
    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                EquipSword();
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                if (isBowUnlocked)
                {
                    EquipBow();
                }
                else
                {
                    Debug.Log("Łuk zablokowany");
                }
                
            }
        }
        

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            float currentCooldown = (currentWeapon == WeaponType.Sword) ? swordCooldown : bowCooldown;
            if (Time.time >= lastAttackTime + currentCooldown)
            {   
                PerformAttack(); 
            }
        }
    }

    private void PerformAttack()
    {
        lastAttackTime = Time.time;

        if (animator == null) return;
        if (currentWeapon == WeaponType.Sword)
        {
            animator.SetTrigger(AttackHash);
        } else if (currentWeapon == WeaponType.Bow)
        {   
            visualArrow.SetActive(true);
            animator.SetTrigger(BowShootHash);
            if (bowAnimator != null)
            {
                bowAnimator.SetTrigger(BowShootHash);
            }
        }
        
    }
    public void EquipSword()
    {
        currentWeapon = WeaponType.Sword;
        if(sword != null) sword.SetActive(true);
        if(bow != null) bow.SetActive(false);
    }
    public void EquipBow()
    {
        if (!isBowUnlocked) return;
        currentWeapon = WeaponType.Bow;
        if(sword != null) sword.SetActive(false);
        if(bow != null) bow.SetActive(true);
    }
    public void OnAttackHit(){
        CameraShake.Instance.Shake(0.1f, 0.04f);
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

    public void OnShootArrow()
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            CameraShake.Instance.Shake(0.08f, 0.04f);
            if(visualArrow != null) visualArrow.SetActive(false);
            
            Camera mainCam = Camera.main;
            if (mainCam == null) return;
            
            Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 100f) ? hit.point : ray.GetPoint(100f);
            
            int ignorePlayerMask = ~LayerMask.GetMask("Player");

            if (Physics.Raycast(ray, out RaycastHit hit2, 100f, ignorePlayerMask))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(100f);
            }
            
            Vector3 shootDirection = (targetPoint - arrowSpawnPoint.position).normalized;
            
            
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.LookRotation(shootDirection));
            Collider arrowCol = arrow.GetComponent<Collider>();

            CharacterController playerController = GetComponent<CharacterController>();
            Collider[] playerCols = GetComponentsInChildren<Collider>();

            if (arrowCol != null)
            {
                if (playerController != null) Physics.IgnoreCollision(arrowCol, playerController);
                foreach (Collider col in playerCols)
                {
                    Physics.IgnoreCollision(arrowCol,col);
                }
            }
            
            Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
            if (arrowRb != null)
            {
                arrowRb.linearVelocity = shootDirection * arrowForce;
            }
        }
    }

    public void UnlockBow()
    {
        isBowUnlocked = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackCenter = transform.position + (transform.forward * attackDistance) + (Vector3.up * 0.8f);
        Gizmos.DrawWireSphere(attackCenter,attackRadius);
    } 
}
