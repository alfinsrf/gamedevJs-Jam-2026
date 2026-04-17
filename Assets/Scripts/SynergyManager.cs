using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    public static SynergyManager Instance { get; private set; }

    private readonly Vector2Int[] _directions = new Vector2Int[]
    {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
    public void EvaluateCellAndNeighbors(Vector2Int targetPos)
    {
        BuildingBase centerBuilding = GetBuildingAt(targetPos);
        if (centerBuilding != null)
        {
            EvaluateBuilding(centerBuilding);
        }
        
        foreach (Vector2Int dir in _directions)
        {
            BuildingBase neighbor = GetBuildingAt(targetPos + dir);
            if (neighbor != null)
            {
                EvaluateBuilding(neighbor);
            }
        }
    }

    private void EvaluateBuilding(BuildingBase building)
    {
        int quarries = 0;
        int batteries = 0;

        foreach (Vector2Int dir in _directions)
        {
            BuildingBase neighbor = GetBuildingAt(building.GridPosition + dir);
            if (neighbor != null)
            {
                if (neighbor.Type == BuildingType.Quarry) quarries++;
                else if (neighbor.Type == BuildingType.BatteryBank) batteries++;
            }
        }
        
        building.UpdateSynergy(quarries, batteries);
    }

    private BuildingBase GetBuildingAt(Vector2Int pos)
    {
        GridCell cell = GridManager.Instance.GetCell(pos.x, pos.y);
        if (cell != null && cell.OccupyingBuilding != null)
        {            
            return cell.OccupyingBuilding.GetComponent<BuildingBase>();
        }
        return null;
    }
}