using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("GameObject que contiene todo el panel del menú de pausa (Canvas/Panel)")]
    public GameObject pauseMenuUI;

    [Tooltip("Botón que se selecciona por defecto cuando se abre el menú")]
    public GameObject defaultSelectedButton;

    [Header("Opciones")]
    [Tooltip("Si true, se pausan también los audios (AudioListener.pause)")]
    public bool pauseAudio = true;

    bool isPaused = false;

    void Start()
    {
        // Asegurarse que el menú inicia oculto y tiempo normal
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Detecta ESC (usa Input System clásica). Si usas el nuevo Input System, reemplaza por tu callback.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);

        // Pausar la simulación física/tiempo
        Time.timeScale = 0f;
        isPaused = true;

        // Pausar audio globalmente (opcional)
        if (pauseAudio) AudioListener.pause = true;

        // Mostrar cursor y desbloquear
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Seleccionar el primer botón para navegación con teclado/joystick
        if (defaultSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        if (pauseAudio) AudioListener.pause = false;

        // Opcional: volver a bloquear cursor si tu juego usa cursor bloqueado
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Deseleccionar para evitar inputs residuales
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    // Botón: Volver al menú principal (asegúrate de que exista una escena con ese nombre)
    public void LoadMainMenu(string mainMenuSceneName)
    {
        // Antes de cargar, restablecemos timeScale
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Botón: Salir de la aplicación (en editor no cierra; usa Application.Quit)
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
