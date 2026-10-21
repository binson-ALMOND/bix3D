using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

public class PlayerPause : MonoBehaviour
{
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Canvas playerUI;
    
    private bool isGamePaused = false;

    private void Start()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.gameObject.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if ((Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame) && pauseCanvas != null)
        {
            if (!isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();

            }
        }
    }

    public void ShowSettings()
    {
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        if (settingsPanel != null)
        {
            settingsPanel.gameObject.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        if (settingsPanel != null)
        {
            settingsPanel.gameObject.SetActive(false);
        }
    }

    private void PauseGame()
    {
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        if(playerUI != null) playerUI.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        if(playerUI != null) playerUI.gameObject.SetActive(true);
        pauseCanvas.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isGamePaused = false;
        Time.timeScale = 1f;
    }
    
    
    
}
