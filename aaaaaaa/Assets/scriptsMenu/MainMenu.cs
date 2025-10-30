using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Nombre de la escena del juego")]
    [Tooltip("Escribe aquí el nombre exacto de la escena de juego (por ejemplo, 'Casa')")]
    [SerializeField] private string gameSceneName = "Casa";

    // Llama a este método desde el botón JUGAR
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Llama a este método desde el botón SALIR
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Si estás en el editor, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estás en una build, cierra la aplicación
        Application.Quit();
#endif
    }
}

