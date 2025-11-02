using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actualMode : MonoBehaviour
{
    public static actualMode Instance { get; private set; }

    public bool isInspecting = false;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            
            Destroy(gameObject);
            return;
        }

        
        Instance = this;
    }

    public void IsInspecting(bool reference)
    {
        isInspecting = reference;
    }
    
}
