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
        // Asegurarse de que el panel de pausa esté inicialmente oculto
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Intentar obtener la instancia persistente del MenuManager
        menuManager = MenuManager.Instance;
        if (menuManager == null)
        {
            Debug.LogError("MenuManager no encontrado. Asegúrate de que existe en la escena inicial.");
        }

        // Asegurarse de que el juego no empiece pausado
        // y que el cursor esté bloqueado/oculto al inicio del juego
        Time.timeScale = 1f;
        isPaused = false;
        SetCursorState(false); // Cursor oculto y bloqueado al empezar
    }

    void Update()
    {
        // 2. Revisar constantemente si el jugador presiona 'Escape'
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

    /// <summary>
    /// Pausa el juego (abre el panel de pausa).
    /// </summary>
    public void PauseGame()
    {
        if (pausePanel == null) return;

        // Mostrar el panel de pausa (Canvas 2)
        menuManager.ShowPanel(pausePanel);

        // Detener el tiempo del juego
        Time.timeScale = 0f;
        isPaused = true;

        // Hacer visible el cursor para poder interactuar con los botones
        SetCursorState(true);
    }

    /// <summary>
    /// Continúa el juego (cierra el panel de pausa).
    /// </summary>
    public void ResumeGame()
    {
        if (pausePanel == null) return;

        // Ocultar el panel de pausa
        menuManager.HidePanel(pausePanel);

        // Reanudar el tiempo del juego
        Time.timeScale = 1f;
        isPaused = false;

        // Ocultar y bloquear el cursor de nuevo
        SetCursorState(false);
    }

    /// <summary>
    /// Reinicia la escena del juego actual.
    /// </summary>
    public void RestartGame()
    {
        // Reiniciar la escala de tiempo por si acaso
        Time.timeScale = 1f;
        // Cargar la escena actual por su nombre
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Asegurar que el cursor esté oculto al recargar la escena
        SetCursorState(false);
    }

    /// <summary>
    /// Sale de la partida y vuelve al menú principal.
    /// (Reutiliza el método del MenuManager, como sugeriste)
    /// </summary>
    public void ExitToMenu()
    {
        if (menuManager != null)
        {
            menuManager.ReturnToMenu();
        }

        // Opcional: Asegurar que el cursor esté visible al volver al menú principal
        SetCursorState(true);
    }

    /// <summary>
    /// Controla la visibilidad y bloqueo del cursor.
    /// </summary>
    /// <param name="isVisible">True para visible y desbloqueado, False para oculto y bloqueado.</param>
    private void SetCursorState(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
