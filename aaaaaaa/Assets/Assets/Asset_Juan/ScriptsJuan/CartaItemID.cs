using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaItemID : MonoBehaviour
{
    [Tooltip("ID único de esta carta (debe ser del 1 al 8).")]
    public int CardID;

    private bool isCollected = false;

    
    public void Interact()
    {
        if (isCollected)
        {
            Debug.Log($"Carta ID {CardID} ya fue inspeccionada. No se añade al contador.");
            return;
        }

        if (actualMode.Instance != null)
        {
            // 2. Preguntar al Manager si el ID ya fue recolectado (Control Centralizado)
            if (actualMode.Instance.CollectCard(CardID))
            {
                // Solo si es la primera vez que se recolecta este ID:
                isCollected = true;
                Debug.Log($"Carta ID {CardID} inspeccionada y añadida al contador.");

                // 3. Desactivar la interacción (evitando futuros Raycasts)
                gameObject.tag = "Untagged";

                // Opcional: Desactivar el Collider o el MeshRenderer si la carta debe desaparecer.
                // GetComponent<Collider>().enabled = false;
                // GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
