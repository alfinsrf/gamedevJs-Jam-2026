using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance { get; private set; }

    private CardData _pendingCard = null;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
    public void SelectCardForPlacement(CardData card)
    {
        _pendingCard = card;
        Debug.Log($"[Placement] Selected {_pendingCard.CardName}. Waiting for Grid click...");
    }

    private void Update()
    {
        if (_pendingCard == null) return;
        
        if (Input.GetMouseButtonDown(1))
        {
            _pendingCard = null;
            Debug.Log("[Placement] Cancelled card placement.");
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            AttemptPlacement();
        }
    }

    private void AttemptPlacement()
    {        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2Int gridPos = GridManager.Instance.WorldToGrid(mousePos);
        
        if (CardManager.Instance.PlayCard(_pendingCard, gridPos.x, gridPos.y))
        {            
            GridCell cell = GridManager.Instance.GetCell(gridPos.x, gridPos.y);
            if (cell != null && cell.OccupyingBuilding != null)
            {
                BuildingBase building = cell.OccupyingBuilding.GetComponent<BuildingBase>();
                if (building != null)
                {
                    building.Initialize(gridPos);
                }
                
                SynergyManager.Instance.EvaluateCellAndNeighbors(gridPos);
            }

            _pendingCard = null;
        }
        else
        {
            Debug.LogWarning("[Placement] Invalid placement or insufficient funds.");
        }
    }
}