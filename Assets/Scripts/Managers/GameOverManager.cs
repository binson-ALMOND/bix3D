using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject playerUI;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSong;
    [SerializeField] private GameObject musicPlayer;

    [Header("Scenes")] 
    [SerializeField] private string mainMenuSceneName;

    [Header("Victory Timing")] 
    [SerializeField] private float delayBeforeVictoryPanel;
    
    private bool isGameOver = false;

    public void TriggerGameOver()
    {
        GameEnded();
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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    public void TriggerVictory()
    {
        StartCoroutine(VictorySequence());
    }

    private void GameEnded()
    {
        if (isGameOver) return;
        isGameOver = true;
        CameraShake.Instance.StopShake();
        Time.timeScale = 0f;

        if (playerUI != null)
        {
            playerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KeepPlaying()
    {
        if(victoryPanel != null) victoryPanel.SetActive(false);
        if(playerUI!=null) playerUI.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator VictorySequence()
    {
        yield return new WaitForSeconds(delayBeforeVictoryPanel);
        GameEnded();
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }
}
