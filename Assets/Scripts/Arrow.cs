using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Parametry")] 
    [SerializeField] private float damage = 1f;
    [SerializeField] private float lifetime = 5f;
    
    private Rigidbody rb;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasHit && rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            transform.forward = rb.linearVelocity.normalized;
        }         
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit || collision.gameObject.CompareTag("Player")) return;
        hasHit = true;
        IDamageable target = collision.gameObject.GetComponent<IDamageable>() ?? collision.gameObject.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        transform.parent = collision.transform;
        
        Destroy(gameObject, lifetime);
    }
}
