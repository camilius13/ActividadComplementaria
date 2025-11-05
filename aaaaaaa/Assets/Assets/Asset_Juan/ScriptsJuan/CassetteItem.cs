using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteItem : MonoBehaviour
{
    [Tooltip("El ID único de este cassette (1, 2, 3 o 4) para la reproducción del video.")]
    [SerializeField]
    private int cassetteID;

    // Propiedad pública de solo lectura
    public int CassetteID => cassetteID;

    private void OnValidate()
    {
        // Asegurar que el ID esté en el rango esperado (Opcional, pero bueno para la organización)
        if (cassetteID < 1 || cassetteID > 4)
        {
            Debug.LogWarning($"El ID del Cassette en {gameObject.name} está fuera del rango (1-4).");
        }
    }
}
