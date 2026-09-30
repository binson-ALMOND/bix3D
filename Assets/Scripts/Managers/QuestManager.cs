using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    [Header("References")] 
    [SerializeField] private PlayerCombat playerCombat;
    [Header("Quest Settings")] 
    public bool isQuestActive = false;
    public bool isQuestCompleted = false;
    public int slimesKilled = 0;
    public int targetSlimes = 5;
    [Header("UI Elements")]
    [SerializeField] private GameObject bowUI;
    [SerializeField] private GameObject exMark;
    [SerializeField] private GameObject questMenu;
    [SerializeField] private TextMeshProUGUI questText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (playerCombat == null)
        {
            playerCombat = FindAnyObjectByType<PlayerCombat>();
        }
        if(questMenu != null) questMenu.SetActive(false);
    }

    public void StartSlimeQuest()
    {
        
        if(exMark!=null) exMark.SetActive(false);
        if (isQuestActive || isQuestCompleted) return;
        isQuestActive = true;
        slimesKilled = 0;
        if(questMenu!=null) questMenu.SetActive(true);
        UpdateQuestUI();

    }

    private void UpdateQuestUI()
    {
        if (questText != null)
        {
            questText.text = $"Slimes killed: {slimesKilled}/{targetSlimes}";
        }
    }
    public void RegisterSlimeKill()
    {
        if(!isQuestActive || isQuestCompleted) return;
        slimesKilled++;
        UpdateQuestUI();
        if (slimesKilled >= targetSlimes)
        {
            CompleteQuest();
        }
    }
    private void CompleteQuest()
    {
        isQuestActive = false;
        isQuestCompleted = true;
        Debug.Log("Quest Complete!");
        if (playerCombat != null)
        {
            playerCombat.UnlockBow();
        }
        
        if(bowUI!=null) bowUI.SetActive(true);
        Invoke(nameof(HideQuestMenu), 3.5f);
    }

    private void HideQuestMenu()
    {
        if(questMenu!=null) questMenu.SetActive(false);
    }
}
