using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayGame()
    {
        MenuManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        MenuManager.Instance.QuitGame();
    }
}
