using UnityEngine;

public enum BuildingType
{
    General,
    Quarry,
    Kiln,
    Weapon,
    BatteryBank
}

public abstract class BuildingBase : MonoBehaviour
{
    [Header("Base Properties")]
    public BuildingType Type = BuildingType.General;
    public int Health = 100;
    public int PowerConsumption = 10;
    public bool IsPowered = true;

    public Vector2Int GridPosition { get; protected set; }

    public virtual void Initialize(Vector2Int gridPos)
    {
        GridPosition = gridPos;
    }
    
    public virtual void UpdateSynergy(int adjacentQuarries, int adjacentBatteries)
    {
        
    }
}