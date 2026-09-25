using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Parametry")] 
    [SerializeField] private float damage = 1f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private AudioSource flyAudioSource; 
    [SerializeField] private AudioClip impactSound;
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
        if(flyAudioSource != null) flyAudioSource.Stop();
        if (impactSound != null)
        {
            flyAudioSource.pitch = Random.Range(0.9f, 1.1f);
            AudioSource.PlayClipAtPoint(impactSound, transform.position,1);
        }

        float finalDamage = damage;
        string hitPart = collision.collider.gameObject.name;
        if (hitPart.Contains("Head"))
        {
            finalDamage *= 2f;
            Debug.Log("Headshot, dmg *2");
        }
        IDamageable target = collision.gameObject.GetComponent<IDamageable>() ?? collision.gameObject.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(finalDamage);
        }

        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        transform.SetParent(collision.transform);
        
        Destroy(gameObject, lifetime);
    }
}
