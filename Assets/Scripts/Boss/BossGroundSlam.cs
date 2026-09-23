using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class BossGroundSlam : MonoBehaviour
{
    [Header("Ustawienia Prefaba")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private LayerMask groundLayer;

    [Header("Parametry Fali")] 
    [SerializeField] private int spikeCount = 8;
    [SerializeField] private float spikeSpacing = 1.6f;
    [SerializeField] private float delayBetweenSpikes = 0.06f;
    [SerializeField] private float startOffset = 2.0f;

    public void SpawnGroundSlamShockwave()
    {
        StartCoroutine(SpawnSpikesRoutine());
    }

    private IEnumerator SpawnSpikesRoutine()
    {
        Vector3 forwardDir = transform.forward;
        Vector3 startOrigin = transform.position + forwardDir*startOffset;

        for (int i = 0; i < spikeCount; i++)
        {
            Vector3 targetXz = startOrigin + forwardDir * (i*spikeSpacing);

            if (Physics.Raycast(targetXz + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f, groundLayer))
            {
                Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(rockPrefab, hit.point, randomRot);
            }
            yield return new WaitForSeconds(delayBetweenSpikes);
        }
    }
}
