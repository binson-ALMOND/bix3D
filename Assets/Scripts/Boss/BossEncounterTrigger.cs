using UnityEngine;

public class BossEncounterTrigger : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private GameObject bossRoot;
    [SerializeField] private GameObject arenaCollider;
    [SerializeField] private Collider detectionCollider;
    
    private Collider[] bossColliders;
    private void Start()
    {
        if (bossHealth == null)
        {
            bossHealth = GetComponentInParent<BossHealth>();
        }

        if (bossRoot != null)
        {
            bossColliders = bossRoot.GetComponentsInChildren<Collider>();

            foreach (Collider col in bossColliders)
            {
                col.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossHealth != null)
            {
                bossHealth.StartEncounter();
            }
            if (arenaCollider != null)
            {
                arenaCollider.SetActive(true);
            }
            if (bossColliders != null)
            {
                foreach (Collider col in bossColliders)
                {
                    if (col != null) col.enabled = true;
                }
            }

            if (detectionCollider != null)
            {
                detectionCollider.enabled = false;
            }
            
        }
    }
}
