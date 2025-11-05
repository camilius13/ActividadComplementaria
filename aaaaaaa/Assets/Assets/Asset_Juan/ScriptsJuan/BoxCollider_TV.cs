using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider_TV : MonoBehaviour
{
    // Ya no necesitamos la etiqueta 'cassetteTag', ya que dependemos del componente.

    private void OnTriggerEnter(Collider other)
    {
        // 1. Intentar obtener el componente que identifica al cassette.
        // Si el objeto que entra no es un Cassette (por ejemplo, es una Carta o el jugador),
        // cassetteItem será 'null' y el código simplemente ignorará el objeto.
        CassetteItem cassetteItem = other.GetComponent<CassetteItem>();

        // 2. Verificar si se encontró el componente CassetteItem.
        if (cassetteItem != null)
        {
            // Hemos confirmado que es un Cassette depositable.
            int cassetteID = cassetteItem.CassetteID;
            Debug.Log($"Cassette ID: {cassetteID} detectado en la zona de depósito.");

            // 3. Llamar al método en el actualMode para aumentar el contador de victoria.
            if (actualMode.Instance != null)
            {
                actualMode.Instance.DepositCassette();
            }

            // 4. Llamar al VideoPlaybackManager para reproducir el video asociado al ID.
            if (VideoPlaybackManager.Instance != null)
            {
                VideoPlaybackManager.Instance.PlayVideoByID(cassetteID);
            }

            // 5. Destruir el objeto Cassette una vez depositado.
            // other.gameObject es el cassette que entró en el trigger.
            Destroy(other.gameObject);

            Debug.Log("Cassette depositado y destruido.");
        }
        // Nota: Si cassetteItem es null (ej. es una Carta), la función termina aquí.
    }
}
