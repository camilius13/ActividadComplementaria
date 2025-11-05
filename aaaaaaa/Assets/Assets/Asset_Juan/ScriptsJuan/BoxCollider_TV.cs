using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider_TV : MonoBehaviour
{// Ya no necesitamos la etiqueta 'cassetteTag', ya que dependemos del componente.

    private void OnTriggerEnter(Collider other)
    {
        // Usar TryGetComponent para verificación segura
        if (other.TryGetComponent<CassetteItem>(out CassetteItem cassetteItem))
        {
            int cassetteID = cassetteItem.CassetteID;
            Debug.Log($"Cassette ID: {cassetteID} detectado en la zona de depósito.");

            // 1. Notificar al modo de juego y actualizar el contador.
            if (actualMode.Instance != null)
            {
                actualMode.Instance.DepositCassette();
            }

            // 2. Iniciar la reproducción del video.
            if (VideoPlaybackManager.Instance != null)
            {
                VideoPlaybackManager.Instance.PlayVideoByID(cassetteID);
            }

            // 3. Destruir el objeto Cassette físico
            Destroy(other.gameObject);

            Debug.Log("Cassette depositado y destruido.");
        }
    }
}
