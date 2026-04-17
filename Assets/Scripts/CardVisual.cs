using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI CostText;
    public Image IconImage;    

    private CardData _myCardData;
    
    public void SetupCard(CardData data)
    {
        _myCardData = data;
        
        if (TitleText != null) TitleText.text = data.CardName;
        if (CostText != null) CostText.text = data.CreditCost.ToString();
        if (IconImage != null) IconImage.sprite = data.CardIcon;
    }
    
    public void OnCardClicked()
    {
        if (_myCardData == null) return;
        
        PlacementSystem.Instance.SelectCardForPlacement(_myCardData);
    }
}