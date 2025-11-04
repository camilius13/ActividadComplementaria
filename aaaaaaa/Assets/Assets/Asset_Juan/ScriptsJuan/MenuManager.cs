using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }


    public string gameSceneName = "SampleScene";
    public string menuSceneName = "Menu";

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
   
        EditorApplication.isPlaying = false;
#else
            // Salir de la aplicación cuando es un ejecutable (build)
            Application.Quit();
#endif
    }

    
    public void ShowPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void HidePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
