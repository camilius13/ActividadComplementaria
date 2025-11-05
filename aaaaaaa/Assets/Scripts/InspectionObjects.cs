using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class InspectionObjects : MonoBehaviour
{
    public Transform objectToInspect;

    // Asigna esto en el Inspector para evitar fallos.
    [SerializeField] private PostProcessVolume blurVolume;

    private float rotationX = 0f;
    private float rotationY = 0f;

    public float rotationSpeed = 100f;
    public float returnSpeed = 5f;

    private Quaternion originalRotation;
    public GameObject actualObject; // Objeto instanciado que se está inspeccionando.
    private Vector3 previousMousePosition;

    // NOTA: Se asume que ObjectInspectionable es una clase o struct definida en tu proyecto.
    // private PostProcessVolume blur;

    private void Start()
    {
        // Si no se asignó en el inspector, intenta encontrarlo en la cámara principal (menos fiable)
        if (blurVolume == null)
        {
            blurVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        }

        // Si blurVolume sigue siendo null, habrá un error cuando intentes usarlo.
        if (blurVolume == null)
        {
            Debug.LogError("El PostProcessVolume no está asignado o no se pudo encontrar en la cámara principal.");
        }

        // Guardar la rotación inicial del objeto "contenedor" de inspección.
        originalRotation = objectToInspect.localRotation;
    }

    void Update()
    {
        // Control de Null: Asegurarse de que el Instance exista.
        if (actualMode.Instance != null && actualMode.Instance.isInspecting)
        {
            inspectionMode();
        }

        // Condición para finalizar la inspección.
        if (actualMode.Instance != null && actualMode.Instance.isInspecting && actualObject != null && Input.GetKeyDown(KeyCode.Q))
        {
            endInspection();
        }
    }

    /// <summary>
    /// Inicia el modo de inspección visual.
    /// </summary>
    public void startInspection(ObjectInspectionable data)
    {
        // 1. Bloquear movimiento/cursor del jugador
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Se mantiene visible para poder rotar el objeto.

        // 2. Activar el blur visual
        if (blurVolume != null) blurVolume.enabled = true;

        Debug.Log($"Inspeccionando: {data.category} ({data.description})");

        // 3. Destruir objeto anterior e instanciar el nuevo
        if (actualObject != null) Destroy(actualObject);

        actualObject = Instantiate(data.objectPrefab, objectToInspect);
    }

    /// <summary>
    /// Termina el modo de inspección.
    /// </summary>
    public void endInspection()
    {
        // 1. Desactivar blur y restaurar cursor
        if (blurVolume != null) blurVolume.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Se recomienda ocultar el cursor para FPS.

        // 2. Notificar al Manager
        if (actualMode.Instance != null) actualMode.Instance.SetIsInspecting(false);

        // 3. Limpiar el objeto instanciado
        Destroy(actualObject);
        actualObject = null;
    }

    /// <summary>
    /// Maneja la rotación del objeto durante la inspección.
    /// </summary>
    public void inspectionMode()
    {
        // Lógica de inicio de rotación (guardar posición del ratón)
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        // Lógica de rotación activa (mientras se mantiene pulsado el botón)
        if (Input.GetMouseButton(0))
        {
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;

            // Calcular la rotación basada en el movimiento del ratón
            // Se usa el delta para rotar en lugar de asignar el valor directamente.
            rotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            rotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

            // Aplicar rotación al objeto contenedor (objectToInspect)
            objectToInspect.localRotation = rotation * objectToInspect.localRotation;

            previousMousePosition = Input.mousePosition;
        }
        else
        {
            // Suavizar el retorno a la rotación original cuando no se rota.
            objectToInspect.localRotation = Quaternion.Lerp(
                objectToInspect.localRotation,
                originalRotation,
                Time.deltaTime * returnSpeed
            );
        }
    }
}
