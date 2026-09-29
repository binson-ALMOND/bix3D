using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject playerUI;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSong;
    [SerializeField] private GameObject musicPlayer;

    [Header("Scenes")] 
    [SerializeField] private string mainMenuSceneName;
    
    private bool isGameOver = false;

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        
       

        if (musicPlayer != null)
        {
            musicPlayer.SetActive(false);
        }

        if (audioSource != null && gameOverSong)
        {
            audioSource.clip = gameOverSong;
            audioSource.loop = true;
            audioSource.Play();
        }
        
        
        Time.timeScale = 0f;

        if (playerUI != null)
        {
            playerUI.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
