using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int coins = 0;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip coinPickUpClip;
    [SerializeField] private TextMeshProUGUI coinText;
    public void AddCoins(int amount)
    {
        if (audioSource != null && coinPickUpClip != null)
        {
            audioSource.PlayOneShot(coinPickUpClip);
        }
        coins += amount;
        UpdateUI();
    }

    private void Start()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = $"x{coins}" ;
        }
    }
    public bool TrySpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -=  amount;
            UpdateUI();
            return true;
        }
        return false;
    }
}

