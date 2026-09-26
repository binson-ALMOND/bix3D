using System;
using UnityEngine;
using System.Collections.Generic;

public class BossWeaponHitbox : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;
    private int currentDamage = 1;
    private bool canDealDamage = false;
    private readonly HashSet<GameObject> hitTargets = new HashSet<GameObject>();
    private void Awake()
    {
        if(weaponCollider == null) weaponCollider = GetComponent<Collider>();

        DisableWeaponHitbox();
    }

    public void EnableWeaponHitbox(int damage)
    {
        currentDamage = damage;
        hitTargets.Clear();
        canDealDamage = true;
        if (weaponCollider!=null) weaponCollider.enabled = true;
    }

    public void DisableWeaponHitbox()
    {
        canDealDamage = false;
        hitTargets.Clear();
        if(weaponCollider!=null) weaponCollider.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage) return;

        if (other.CompareTag("Player") && !hitTargets.Contains(other.gameObject))
        {
            hitTargets.Add(other.gameObject);
            other.GetComponent<PlayerHealth>().TakeDamage(currentDamage);
            Debug.Log($"Cios zadał {currentDamage}");
        }
    }
}
