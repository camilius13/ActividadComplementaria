using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Nombre de la escena del juego")]
    [Tooltip("Escribe aqu� el nombre exacto de la escena de juego (por ejemplo, 'Casa')")]
    [SerializeField] private string gameSceneName = "Casa";

    // Llama a este m�todo desde el bot�n JUGAR
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Llama a este m�todo desde el bot�n SALIR
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Si est�s en el editor, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si est�s en una build, cierra la aplicaci�n
        Application.Quit();
#endif
    }
}

