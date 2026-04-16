using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [Header("Deck Configuration")]
    public List<CardData> StartingDeck;

    // lists
    private List<CardData> _drawPile = new List<CardData>();
    private List<CardData> _hand = new List<CardData>();
    private List<CardData> _discardPile = new List<CardData>();

    // Public accessors
    public IReadOnlyList<CardData> Hand => _hand;
    public int DrawPileCount => _drawPile.Count;
    public int DiscardPileCount => _discardPile.Count;

    // Events
    public event Action<List<CardData>> OnHandUpdated;
    public event Action<int, int> OnDeckStateChanged; // Draw count, Discard count

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        _drawPile.Clear();
        _hand.Clear();
        _discardPile.Clear();

        _drawPile.AddRange(StartingDeck);
        ShuffleDeck();

        // Draw starting hand
        DrawCard(5);
    }

    public void ShuffleDeck()
    {        
        for (int i = _drawPile.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            CardData temp = _drawPile[i];
            _drawPile[i] = _drawPile[j];
            _drawPile[j] = temp;
        }
    }

    public void DrawCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_drawPile.Count == 0)
            {
                if (_discardPile.Count == 0) break; // Entire deck is in hand

                // Reshuffle discard into draw
                _drawPile.AddRange(_discardPile);
                _discardPile.Clear();
                ShuffleDeck();
            }

            CardData drawn = _drawPile[0];
            _drawPile.RemoveAt(0);
            _hand.Add(drawn);
        }

        NotifyUI();
    }

    public bool PlayCard(CardData card, int targetGridX, int targetGridY)
    {
        if (!_hand.Contains(card)) return false;

        // 1. Cost Validation
        if (ResourceManager.Instance.Credits < card.CreditCost)
        {
            Debug.LogWarning($"Not enough credits to play {card.CardName}");
            return false;
        }

        // Ensure playing it doesn't cause a short-circuit/brownout
        if (ResourceManager.Instance.CurrentLoad + card.EnergyCost > ResourceManager.Instance.MaxPower)
        {
            Debug.LogWarning($"Not enough power capacity to play {card.CardName}");
            return false;
        }

        // 2. Target Validation
        if (!GridManager.Instance.IsCellBuildable(targetGridX, targetGridY))
        {
            Debug.LogWarning("Target cell is either occupied or out of bounds.");
            return false;
        }

        // 3. Complete Transaction
        ResourceManager.Instance.AddCredits(-card.CreditCost);
        ResourceManager.Instance.UpdatePowerLoad(card.EnergyCost);

        // 4. Place Building
        Vector3 spawnPos = GridManager.Instance.GridToWorld(targetGridX, targetGridY);
        GameObject newBuilding = Instantiate(card.BuildingPrefab, spawnPos, Quaternion.identity);
        GridManager.Instance.GetCell(targetGridX, targetGridY).OccupyingBuilding = newBuilding;

        // 5. Move to Discard
        _hand.Remove(card);
        _discardPile.Add(card);

        NotifyUI();
        return true;
    }

    private void NotifyUI()
    {
        OnHandUpdated?.Invoke(_hand);
        OnDeckStateChanged?.Invoke(_drawPile.Count, _discardPile.Count);
    }
}