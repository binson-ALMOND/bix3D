using UnityEngine;
using TMPro;
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int coins = 0;

    [SerializeField] private TextMeshProUGUI coinText;
    public void AddCoins(int amount)
    {
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

