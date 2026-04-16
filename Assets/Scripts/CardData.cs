using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "New Game Data/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Card Identity")]
    public string CardName;
    [TextArea(2, 4)] public string Description;
    public Sprite CardIcon;        
    public int CreditCost;    
    public int EnergyCost;        
    public GameObject BuildingPrefab;
}