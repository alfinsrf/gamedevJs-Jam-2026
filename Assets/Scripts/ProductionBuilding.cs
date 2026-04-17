using System.Collections;
using UnityEngine;

public class ProductionBuilding : BuildingBase
{
    [Header("Production Settings")]
    public ResourceType ResourceProduced;
    public float BaseProductionTick = 2f;
    public float BaseProductionAmount = 1f;

    private float _synergyMultiplier = 1f;

    public override void Initialize(Vector2Int gridPos)
    {
        base.Initialize(gridPos);
        StartCoroutine(ProductionRoutine());
    }

    public override void UpdateSynergy(int adjacentQuarries, int adjacentBatteries)
    {        
        if (Type == BuildingType.Kiln)
        {
            _synergyMultiplier = 1f + (0.2f * adjacentQuarries);
            Debug.Log($"[Synergy] {gameObject.name} at {GridPosition} synergy updated. Multiplier: {_synergyMultiplier}");
        }
    }

    private IEnumerator ProductionRoutine()
    {
        while (true)
        {            
            yield return new WaitForSeconds(BaseProductionTick);

            if (!IsPowered) continue;
            
            float brownoutPenalty = ResourceManager.Instance.IsInBrownout ? 0.5f : 1f;
            int finalAmount = Mathf.RoundToInt(BaseProductionAmount * _synergyMultiplier * brownoutPenalty);

            if (finalAmount > 0)
            {
                ResourceManager.Instance.AddResource(ResourceProduced, finalAmount);
            }
        }
    }
}