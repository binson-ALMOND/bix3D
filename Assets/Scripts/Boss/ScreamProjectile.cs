using UnityEngine;

public class ScreamProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float speed = 14f;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    
    void Update()
    {
        transform.Translate(Vector3.forward  * (speed * Time.deltaTime));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Debug.Log($"Krzyk bije za {damage}");
            Destroy(gameObject);
        }
    }
}
