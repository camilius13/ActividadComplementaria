using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class objectGrabbable : MonoBehaviour
{

    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    [SerializeField] private Transform playerCameraTransform;

    public float lerpSpeed = 10f;


    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        // Intenta encontrar la cámara principal si no está asignada en el Inspector.
        if (playerCameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                playerCameraTransform = mainCam.transform;
            }
            else
            {
                Debug.LogWarning("No se encontró una cámara principal para la lógica de agarre.");
            }
        }
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = true;
        gameObject.transform.SetParent(objectGrabPointTransform);

        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        gameObject.transform.SetParent(null);
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        // Aplicar un pequeño impulso al soltar.
        objectRigidbody.AddForce(playerCameraTransform.forward * 1f, ForceMode.VelocityChange);

    }

    public void Throw(float throwForce)
    {
        gameObject.transform.SetParent(null);


        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        objectRigidbody.AddForce(playerCameraTransform.forward * throwForce, ForceMode.VelocityChange);

        this.objectGrabPointTransform = null;

    }
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            // Usar la asignación directa de posición para mantener el objeto exactamente en el punto de agarre.
            transform.position = objectGrabPointTransform.transform.position;
        }
    }
}
