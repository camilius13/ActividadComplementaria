using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class InspectionObjects : MonoBehaviour
{
    public Transform objectToInspect;
    private float rotationX = 0f;
    private float rotationY = 0f;

    public float rotationSpeed = 100f;

    public float returnSpeed = 5f;

    private Quaternion originalRotation;

    public GameObject actualObject;

    private Vector3 previousMousePosition;

    private PostProcessVolume blur;

    private void Start()
    {
        blur = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        originalRotation = objectToInspect.localRotation;
    }

    void Update()
    {
        if (actualMode.Instance.isInspecting)
        {
            inspectionMode();
        }

        if(actualMode.Instance.isInspecting && actualObject !=null && Input.GetKeyDown(KeyCode.Q))
        {
            endInspection();
        }

    }

    public void startInspection(ObjectInspectionable data)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        blur.enabled = true;

       Debug.Log($"Inspeccionando: {data.category} ({data.description})");

       if(actualObject != null) Destroy(actualObject);

       actualObject = Instantiate(data.objectPrefab,objectToInspect);
    
    }

    public void endInspection()
    {
        blur.enabled = false;   
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        actualMode.Instance.IsInspecting(false);
        Destroy(actualObject);
        actualObject = null;
    }
    public void inspectionMode()
    {

        
        if (Input.GetMouseButtonDown(0))
        {
            
            previousMousePosition = Input.mousePosition;
        }

        

        if(Input.GetMouseButton(0))
        {
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
            

            rotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            rotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;
            
            Debug.Log(rotationX + ", " + rotationY);

            //rotationX = Mathf.Clamp(rotationX, -1.5f, 1.5f);
            //rotationY = Mathf.Clamp(rotationY, -1.5f, 1.5f);

            Debug.Log(rotationX + ", " + rotationY);

            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            objectToInspect.localRotation = rotation * objectToInspect.localRotation;

            previousMousePosition = Input.mousePosition;
        }
        else
        {
            objectToInspect.localRotation = Quaternion.Lerp(
                objectToInspect.localRotation,
                originalRotation,
                Time.deltaTime * returnSpeed
            );
        }
    }
}
