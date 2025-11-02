using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InspectionObjects : MonoBehaviour
{
    public Transform objectInspect;

    public float rotationSpeed = 100f;

    private Vector3 previousMousePosition;

    public 
    
    void Update()
    {
        if (actualMode.Instance.isInspecting)
        {

        }
    }

    public void inspect(GameObject Obj)
    {
      
        objectInspect = Obj.transform;
    
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
            float rotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            float rotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            objectInspect.rotation = rotation;

            previousMousePosition = Input.mousePosition;
        }
    }
}
