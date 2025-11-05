using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

    public class actualMode : MonoBehaviour
    {
        public static actualMode Instance { get; private set; }
        public bool isInspecting;
        public TextMeshProUGUI cassetteText;
        public TextMeshProUGUI cardText;
        private int cassetteCount;
        private const int MAX_CASSETTES_TO_WIN = 4;
        private int cardCount;
        private const int MAX_CARDS = 8;
        private HashSet<int> collectedCards;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        public void IsInspecting(bool reference)
        {
            isInspecting = reference;
        }

        public void SetIsInspecting(bool value)
        {
            isInspecting = value;
        }

        public void DepositCassette()
        {
            if (cassetteCount < MAX_CASSETTES_TO_WIN)
            {
                cassetteCount++;
                UpdateUI();
                CheckWinCondition();
            }
        }

        public bool CollectCard(int cardID)
        {
        Debug.Log($"Intentando recolectar carta ID {cardID}.");
        if (collectedCards == null)
            {
                collectedCards = new HashSet<int>();
            }

            if (collectedCards.Count < MAX_CARDS && collectedCards.Add(cardID))
            {
            Debug.Log($"Carta ID {cardID} recolectada. Total cartas: {collectedCards.Count}");
            cardCount++;
                UpdateUI();
                return true;
            }

            return false;
        }

        private void UpdateUI()
        {
            if (cassetteText != null)
            {
                cassetteText.text = $"Cassettes: {cassetteCount}/{MAX_CASSETTES_TO_WIN}";
            }

            if (cardText != null)
            {
                cardText.text = $"Cards: {cardCount}/{MAX_CARDS}";
            }
        }

        private void CheckWinCondition()
        {
            if (cassetteCount >= MAX_CASSETTES_TO_WIN)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            Debug.Log("Game Over! You collected all the cassettes.");
        }
    }
