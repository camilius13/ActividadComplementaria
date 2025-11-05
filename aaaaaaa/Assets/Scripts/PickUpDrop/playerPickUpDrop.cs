


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPickUpDrop : MonoBehaviour
{

    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private InspectionObjects inspectObject;

    private objectGrabbable objectGrabbable;
    public float pickupDistance = 2f;
    public float throwForce = 6f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                // Sin objeto en la mano: Intento de agarrar
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }
            else
            {
                // Objeto en la mano: Soltar
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
        else if (objectGrabbable != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Objeto en la mano: Lanzar
            objectGrabbable.Throw(throwForce);
            objectGrabbable = null;
        }
        // --- BLOQUE DE INSPECCIÓN CORREGIDO (Tecla I) ---
        else if (objectGrabbable == null && Input.GetKeyDown(KeyCode.I))
        {
            // Lanzar Raycast para inspeccionar
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, pickupDistance))
            {
                // Verificar si el objeto tiene el Tag correcto
                if (hit.transform.CompareTag("inspectObject"))
                {
                    // 1. Intentar obtener los datos de inspección visual de forma SEGURA (evita NullReference)
                    if (hit.collider.TryGetComponent<inspectionData>(out inspectionData inspection))
                    {
                        // Iniciar el modo de inspección visual
                        if (actualMode.Instance != null)
                        {
                            // La función que tienes se llama IsInspecting, no SetIsInspecting
                            actualMode.Instance.IsInspecting(true);
                        }

                        // Iniciar la inspección visual
                        inspectObject.startInspection(inspection.getData());

                        // 2. Intentar obtener el componente de la Carta (CartaItemID)
                        // ESTO ES LO QUE EJECUTA EL CONTADOR Y LA LÓGICA DE COLECCIÓN.
                        if (hit.collider.TryGetComponent<CartaItemID>(out CartaItemID cardItem))
                        {
                            // Llamar al método que procesa el ID y actualiza el contador
                            cardItem.Interact();
                        }
                    }
                    else
                    {
                        // Este Debug ayuda a identificar objetos mal configurados
                        Debug.LogWarning($"Objeto con tag 'inspectObject' pero sin componente 'inspectionData' en: {hit.collider.gameObject.name}");
                    }
                }
            }
        }
    }

}



