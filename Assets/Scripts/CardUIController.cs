using System.Collections.Generic;
using UnityEngine;

public class CardUIController : MonoBehaviour
{
    [Header("UI References")]
    public Transform HandContainer;
    public GameObject CardPrefab; // image

    [Header("Optimization")]    
    public int MaxHandSize = 10;

    private List<GameObject> _cardPool = new List<GameObject>();

    private void OnEnable()
    {        
        if (CardManager.Instance != null)
        {
            Debug.Log("[CardUIController] Successfully subscribed to CardManager events!");
            CardManager.Instance.OnHandUpdated += RefreshHandUI;
        }
    }

    private void OnDisable()
    {        
        if (CardManager.Instance != null)
            CardManager.Instance.OnHandUpdated -= RefreshHandUI;
    }

    private void Awake()
    {
        Debug.Log("[CardUIController] Setting up Object Pool");        
        for (int i = 0; i < MaxHandSize; i++)
        {
            GameObject cardUI = Instantiate(CardPrefab, HandContainer);
            cardUI.SetActive(false);
            _cardPool.Add(cardUI);
        }
    }

    private void Start()
    {        
        // If CardManager is already initialized, sync UI immediately
        if (CardManager.Instance != null)
            RefreshHandUI((List<CardData>)CardManager.Instance.Hand);
    }

    private void RefreshHandUI(List<CardData> currentHand)
    {
        Debug.Log($"[CardUIController] Refreshing UI. Cards in hand: {currentHand.Count}");
        
        foreach (var ui in _cardPool)
        {
            ui.SetActive(false);
        }
        
        for (int i = 0; i < currentHand.Count; i++)
        {
            if (i >= _cardPool.Count) {
                Debug.LogWarning("[CardUIController] Hand size exceeds Pool size!");
                break; 
            }

            GameObject cardObj = _cardPool[i];
            cardObj.SetActive(true);            
            cardObj.name = currentHand[i].CardName;            
        }
    }
    
    public void OnDrawCardButtonClicked()
    {
        Debug.Log("[CardUIController] Draw Button Clicked!");
        CardManager.Instance.DrawCard(1);
    }
}