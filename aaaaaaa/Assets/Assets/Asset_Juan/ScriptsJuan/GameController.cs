using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Referencias de Canvas")]
    [Tooltip("Arrastra aquí el GameObject del panel de Pausa (Canvas 2).")]
    public GameObject pausePanel;

    private MenuManager menuManager;
    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        menuManager = MenuManager.Instance;
        if (menuManager == null)
        {
            Debug.LogError("MenuManager no encontrado. Asegúrate de que existe en la escena inicial.");
        }

        Time.timeScale = 1f;
        isPaused = false;
        SetCursorState(false); // Cursor oculto y bloqueado al empezar
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    
    public void PauseGame()
    {
        if (pausePanel == null) return;

        menuManager.ShowPanel(pausePanel);

        Time.timeScale = 0f;
        isPaused = true;

        SetCursorState(true);
    }

   
    public void ResumeGame()
    {
        if (pausePanel == null) return;

        menuManager.HidePanel(pausePanel);

        Time.timeScale = 1f;
        isPaused = false;

        SetCursorState(false);
    }

    
    public void RestartGame()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        
        SetCursorState(false);
    }

    
    public void ExitToMenu()
    {
        if (menuManager != null)
        {
            menuManager.ReturnToMenu();
        }

        
        SetCursorState(true);
    }

    private void SetCursorState(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
