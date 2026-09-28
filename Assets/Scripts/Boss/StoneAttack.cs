using UnityEngine;
using System.Collections;

public class StoneAttack : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float popUpHeight = 2f;
    [SerializeField] private float popUpSpeed = 16f;
    [SerializeField] private float lifetime = 3.5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip emergeSFX;
    [SerializeField] private AudioClip hideSFX;
    private Vector3 targetPosition;
    private bool hasDealtDamage = false;
    
    private void Start()
    {
        targetPosition = transform.position;
        transform.position = targetPosition - Vector3.up * popUpHeight;

        StartCoroutine(EmergeRoutine());
        Destroy(gameObject, lifetime);
    }


    private IEnumerator EmergeRoutine()
    {
        if (emergeSFX != null)
        {
            audioSource.PlayOneShot(emergeSFX);
        }
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPosition,popUpSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        if (hideSFX != null)
        {
            yield return new WaitForSeconds(lifetime-0.5f);
            audioSource.PlayOneShot(hideSFX);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasDealtDamage) return;
        if (other.CompareTag("Player"))
        {
            hasDealtDamage = true;
            Debug.Log($"Rock hits for {damage}!");
            
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
