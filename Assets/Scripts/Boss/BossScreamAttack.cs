using UnityEngine;

public class BossScreamAttack : MonoBehaviour
{
    [Header("Referencje")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Ustawienia rozrzutu")] 
    [SerializeField] private float spreadAngle = 22.5f;
    [SerializeField] private float heightOffset = 1.8f;

    public void FireScream()
    {
        if (projectilePrefab == null) return;
        Vector3 spawnPosition = transform.position + Vector3.up * heightOffset + transform.forward * 1.0f;

        float[] angles = {-spreadAngle, 0, spreadAngle};
        foreach (float angle in angles)
        {
            Quaternion rotationOffset = Quaternion.Euler(0, angle, 0);
            Quaternion finalRotation = transform.rotation * rotationOffset;
            
            Instantiate(projectilePrefab, spawnPosition, finalRotation);
        }
    }
}
