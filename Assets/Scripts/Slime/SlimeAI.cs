using UnityEngine;
using System.Collections;

public class SlimeAI : MonoBehaviour
{
   
    [Header("Wykrywanie Gracza")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float detectionRadius = 12f;
    [SerializeField] private float stopDistance = 1.2f;

    [Header("Fizyka skoku")] 
    [SerializeField] private float jumpForwardForce = 5f;
    [SerializeField] private float jumpUpwardForce = 4.5f;
    [SerializeField] private float jumpCooldown = 1.5f;
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Animacje")] 
    [SerializeField] private Animator animator;
    [SerializeField] private string jumpTriggerName = "Jump";

    [Header("Obrażenia")] [SerializeField] private float slimeDamage = 1;
    [Header("SFX")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioSource audioSource;
    
    private Transform player;
    private Rigidbody rb;
    private bool isJumping = false;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) player = p.transform;
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            if (!isJumping && isGrounded)
            {
                StartCoroutine(PerformJump(direction));
            }
        }
    }


    private IEnumerator PerformJump(Vector3 targetDirection)
    {
        isJumping = true;
        
        if(animator != null && !string.IsNullOrEmpty(jumpTriggerName))
        {
            animator.SetTrigger(jumpTriggerName);
        }
        
        yield return new WaitForSeconds(jumpCooldown);
        isJumping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.4f)
        {
            isGrounded = true;
            if(landSound != null) audioSource.PlayOneShot(landSound);
        }

        if (collision.gameObject.CompareTag(playerTag))
        {
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(slimeDamage);
            }  
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void OnJumpLaunch()
    {
        if (player == null) return;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        Vector3 jumpForce = (direction * jumpForwardForce) + (Vector3.up * jumpUpwardForce);
        rb.AddForce(jumpForce, ForceMode.Impulse);
        isGrounded = false;
        if(walkSound != null) audioSource.PlayOneShot(walkSound);
    }
}
