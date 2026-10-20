using UnityEngine;
using System.Collections;
public class BossStomp : MonoBehaviour
{
    [Header("Prefab & Ground")]
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wave Settings")] 
    [SerializeField] private int numberOfWaves = 4;
    [SerializeField] private float timeBetweenWaves = 0.2f;
    [SerializeField] private float initialRadius = 2.5f;
    [SerializeField] private float distanceBetweenWaves = 2f;
    
    [Header("Density")]
    [SerializeField] private int baseStoneCount = 8;
    [SerializeField] private int extraStonesPerWave = 4;

    [Header("Stone Behavior")] 
    [SerializeField] private float stoneLifetime = 3f;
    [SerializeField] private Vector3 minScale = new Vector3(.9f, .9f, .9f);
    [SerializeField] private Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
    
    public void TriggerStomp()
    {
        StartCoroutine(SpawnConcentricWaves());
    }

    private IEnumerator SpawnConcentricWaves()
    {
        Vector3 bossCenter = transform.position;
        for (int wave = 0; wave < numberOfWaves; wave++)
        {
            float currentRadius = initialRadius + (wave*distanceBetweenWaves);
            int stonesInThisWave = baseStoneCount + (wave * extraStonesPerWave);

            for (int i = 0; i < stonesInThisWave; i++)
            {
                float angle = i * Mathf.PI * 2 / stonesInThisWave;
                Vector3 offset = new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle)) *currentRadius;
                Vector3 spawnTarget = bossCenter + offset;
                
                Vector3 raycastStart = new Vector3(spawnTarget.x, bossCenter.y + 5f, spawnTarget.z);

                if (Physics.Raycast(raycastStart, Vector3.down, out RaycastHit hit, 15f, groundLayer))
                {
                    Vector3 spawnPosition = hit.point;
                    
                    GameObject stone = Instantiate(stonePrefab, spawnPosition,Quaternion.identity);
                    
                    stone.transform.rotation = Quaternion.Euler(Random.Range(-15f,15f),Random.Range(0f,360f),Random.Range(-15f,15f));
                    
                    float randomScaleMultiplier =  Random.Range(0.8f, 1.2f);
                    stone.transform.localScale = Vector3.Scale(Vector3.Lerp(minScale,maxScale,Random.value),new Vector3(randomScaleMultiplier, randomScaleMultiplier, randomScaleMultiplier));
                    
                    Destroy(stone,stoneLifetime);
                }
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }
       
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 bossCenter = transform.position;
        for (int wave = 0; wave < numberOfWaves; wave++)
        {
            float radius = initialRadius + (wave * distanceBetweenWaves);
            Gizmos.DrawWireSphere(bossCenter, radius);
        }
    }
}
