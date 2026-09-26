using UnityEngine;

public class BossEncounterTrigger : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    private void Start()
    {
        if (bossHealth == null)
        {
            bossHealth = GetComponentInParent<BossHealth>();
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
            gameObject.SetActive(false);
        }
    }
}
