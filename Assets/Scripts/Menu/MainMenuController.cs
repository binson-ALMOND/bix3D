using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Konfiguracja Scen")] 
    [SerializeField] private string gameSceneName = "Dev_BossFight";
    
    [Header("Panele")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        
    }

    public void OpenCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
        
    }

    public void CloseAllPanels()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if(creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
