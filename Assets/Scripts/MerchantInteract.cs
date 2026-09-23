using UnityEngine;
using UnityEngine.InputSystem;
public class MerchantInteract : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private PlayerInventory playerEq;
    private PlayerHealth playerHealth;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerEq = other.GetComponent<PlayerInventory>();
            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerEq = null;
            playerHealth = null;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (playerEq.TrySpendCoins(20) == true)
            {
                playerHealth.HealToMax();
            }
        }
    }
}
