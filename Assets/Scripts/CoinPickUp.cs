using System.Collections;
using TMPro;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Animacja")]
    [SerializeField] private float spinSpeed = 150f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float groundOffset = 0.2f;

    private void Start()
    {
        StartCoroutine(JumpRoutine());
    }

    private void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0, Space.World);
    }

    private IEnumerator JumpRoutine()
    {
        Vector3 startPos = transform.position;
        
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        Vector3 endPos = startPos + randomOffset;
        
        Vector3 rayStartPoint = new Vector3(endPos.x, startPos.y + 1f, endPos.z);
        
        if (Physics.Raycast(rayStartPoint, Vector3.down, out RaycastHit hit, 10f))
        {
            endPos.y = hit.point.y + groundOffset;
        }
        else
        {
            endPos.y = startPos.y - 1f; 
        }

        float time = 0f;

        while (time < jumpDuration)
        {
            time += Time.deltaTime;
            float percent = time / jumpDuration;
            
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, percent);
            
            currentPos.y += Mathf.Sin(percent * Mathf.PI) * jumpHeight;

            transform.position = currentPos;

            yield return null; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory eq = other.GetComponent<PlayerInventory>();
            if (eq != null)
            {
                eq.AddCoins(1);
                Destroy(gameObject);
            }
        }
    }
}