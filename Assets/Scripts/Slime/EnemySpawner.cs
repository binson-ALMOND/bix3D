using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Ustawienia Prefaba")] 
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int maxEnemies = 10;

    [Header("Obszar spawnowania")]
    [SerializeField] private Vector2 areaSize = new Vector2(30f, 30f);
    [SerializeField] private LayerMask groundLayer;

    private int currentEnemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
           

            if (currentEnemyCount < maxEnemies && slimePrefab != null)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randomZ = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        
        Vector3 rayOrigin = transform.position + new Vector3(randomX, 50f, randomZ);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 spawnPosition = hit.point + Vector3.up * 0.2f;
            GameObject newEnemy = Instantiate(slimePrefab, spawnPosition, Quaternion.identity);
            currentEnemyCount++;

            EnemyHealth health = newEnemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                newEnemy.AddComponent<EnemyDeathNotifier>().OnDestroyed += () => currentEnemyCount--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellowGreen;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 1f, areaSize.y));
    }
}

public class EnemyDeathNotifier : MonoBehaviour
{
    public System.Action OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
