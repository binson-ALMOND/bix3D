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
    [SerializeField] private TextMeshProUGUI questCompleteText;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip questCompleteClip;
    private const string SLIME_QUEST_KEY = "Quest_Slime_Completed";
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
        isQuestCompleted = PlayerPrefs.GetInt(SLIME_QUEST_KEY, 0) == 1;
    }

    private void Start()
    {
        if (isQuestCompleted)
        {
            if(exMark!=null) exMark.SetActive(false);
            if(bowUI!=null) bowUI.SetActive(true);
            if(playerCombat!=null) playerCombat.UnlockBow();
        }
    }

    public void StartSlimeQuest()
    {
        if (isQuestActive || isQuestCompleted) return;
        
        if(exMark!=null) exMark.SetActive(false);
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
       
        PlayerPrefs.SetInt(SLIME_QUEST_KEY,1);
        PlayerPrefs.Save();
        if (playerCombat != null)
        {
            playerCombat.UnlockBow();
        }
        if(audioSource!=null) audioSource.PlayOneShot(questCompleteClip,0.2f);
        if(bowUI!=null) bowUI.SetActive(true);
        if (questCompleteText != null) questCompleteText.gameObject.SetActive(true);
        Invoke(nameof(HideQuestMenu), 3.5f);
    }

    private void HideQuestMenu()
    {
        if(questMenu!=null) questMenu.SetActive(false);
    }

    public static void ResetQuestProgress()
    {
        PlayerPrefs.DeleteKey(SLIME_QUEST_KEY);
        PlayerPrefs.Save();
    }
}
