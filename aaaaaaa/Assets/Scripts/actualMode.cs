using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class actualMode : MonoBehaviour
{
    // Singleton (Lógica base del compañero)
    public static actualMode Instance { get; private set; }

    [Header("Estado de Interacción")]
    [Tooltip("Indica si el jugador está actualmente inspeccionando un objeto (para bloquear el movimiento).")]
    public bool isInspecting = false;

    // --- NUEVAS VARIABLES DE JUEGO ---

    [Header("Contadores de Coleccionables")]
    [Tooltip("Referencia al TextMeshPro que muestra el contador de Cassettes.")]
    public TextMeshProUGUI cassetteText;
    [Tooltip("Referencia al TextMeshPro que muestra el contador de Cartas.")]
    public TextMeshProUGUI cardText;

    private int cassetteCount = 0;
    private const int MAX_CASSETTES_TO_WIN = 4; // Condición de victoria estricta

    private int cardCount = 0;
    private const int MAX_CARDS = 8;

    // Almacena los IDs de las cartas ya inspeccionadas para evitar el conteo doble.
    private HashSet<int> collectedCards = new HashSet<int>();

    // --- MÉTODOS BASE ---

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Destruir esta instancia si ya existe otra
            Destroy(gameObject);
            return;
        }

        // Establecer esta como la instancia única
        Instance = this;
    }

    private void Start()
    {
        // Inicializar la interfaz de usuario con los contadores a 0
        UpdateUI();
    }

    public void IsInspecting(bool reference)
    {
        isInspecting = reference;
    }

    // --- NUEVOS MÉTODOS DE LÓGICA DE JUEGO ---

    /// <summary>
    /// Se llama cuando un Cassette es depositado en la zona de trigger.
    /// Esta es la acción que cuenta para la victoria.
    /// </summary>
    public void DepositCassette()
    {
        // Solo incrementar si no hemos ganado aún
        if (cassetteCount < MAX_CASSETTES_TO_WIN)
        {
            cassetteCount++;
            Debug.Log($"Cassette depositado. Total: {cassetteCount}/{MAX_CASSETTES_TO_WIN}");

            UpdateUI();
            CheckWinCondition();
        }
    }

    /// <summary>
    /// Se llama cuando el jugador inspecciona una Carta.
    /// Solo incrementa el contador la primera vez que se inspecciona una carta con un ID único.
    /// </summary>
    /// <param name="cardID">ID único de la carta (1 a 8).</param>
    public void CollectCard(int cardID)
    {
        // 1. Verificar si el ID de la carta ya está en la lista de cartas recolectadas.
        if (cardCount < MAX_CARDS && !collectedCards.Contains(cardID))
        {
            // 2. Si no ha sido recolectada, añadir el ID, incrementar el contador y actualizar UI.
            collectedCards.Add(cardID);
            cardCount++;
            Debug.Log($"Carta inspeccionada por primera vez (ID: {cardID}). Total: {cardCount}/{MAX_CARDS}");

            UpdateUI();
        }
        else
        {
            // Debug para verificar que no suma puntos infinitos
            Debug.Log($"Carta (ID: {cardID}) ya fue inspeccionada. No suma al contador.");
        }
    }

    /// <summary>
    /// Actualiza los textos de la interfaz de usuario con los contadores actuales.
    /// </summary>
    private void UpdateUI()
    {
        if (cassetteText != null)
        {
            cassetteText.text = $"{cassetteCount} / {MAX_CASSETTES_TO_WIN}";
        }

        if (cardText != null)
        {
            cardText.text = $"{cardCount} / {MAX_CARDS}";
        }
    }

    /// <summary>
    /// Chequea si se ha alcanzado la condición de victoria (4 Cassettes depositados).
    /// </summary>
    private void CheckWinCondition()
    {
        if (cassetteCount >= MAX_CASSETTES_TO_WIN)
        {
            Debug.Log("¡CONDICIÓN DE VICTORIA ALCANZADA! El juego ha finalizado.");

            // Llamar al método que maneja el Game Over/Win (Probablemente en el MenuManager o aquí).
            GameOver();
        }
    }

    /// <summary>
    /// Lógica de fin de juego (mostrar Canvas 3, pausar, etc.).
    /// </summary>
    public void GameOver()
    {
        // NOTA: Aquí debes implementar la lógica para:
        // 1. Mostrar el Canvas 3 (Panel de "Ganaste").
        // 2. Pausar el juego (Time.timeScale = 0f;).
        // 3. Mostrar el cursor (SetCursorState(true);).
        // Puedes referenciar y usar el MenuManager.Instance para gestionar esto.

        Time.timeScale = 0f; // Congelar el juego
        // Opcional: Llamar al GameSceneLocalManager para mostrar cursor y ocultar otros paneles
    }

}
