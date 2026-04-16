using UnityEngine;

public class GridCell
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public ResourceType OreNode { get; set; } = ResourceType.None;
    public GameObject OccupyingBuilding { get; set; } = null;

    public GridCell(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IsBuildable()
    {
        return OccupyingBuilding == null;
    }
}

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    public int Width = 30;
    public int Height = 20;
    public float CellSize = 1f;
    public Vector2 OriginPosition = Vector2.zero;

    private GridCell[,] _grid;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        _grid = new GridCell[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _grid[x, y] = new GridCell(x, y);
            }
        }
    }

    public GridCell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            return _grid[x, y];
        return null;
    }
    
    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(x * CellSize + OriginPosition.x + (CellSize / 2f),
                           y * CellSize + OriginPosition.y + (CellSize / 2f), 0);
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - OriginPosition.x) / CellSize);
        int y = Mathf.FloorToInt((worldPosition.y - OriginPosition.y) / CellSize);
        return new Vector2Int(x, y);
    }

    public bool IsCellBuildable(int x, int y)
    {
        var cell = GetCell(x, y);
        return cell != null && cell.IsBuildable();
    }

    // Debugging
    private void OnDrawGizmos()
    {
        if (_grid == null) return;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GridCell cell = _grid[x, y];
                Vector3 worldPos = GridToWorld(x, y);

                // Draw bounds
                Gizmos.color = Color.white * 0.2f;
                Gizmos.DrawWireCube(worldPos, new Vector3(CellSize, CellSize, 0));

                // Draw Ore Nodes
                if (cell.OreNode != ResourceType.None)
                {
                    Gizmos.color = GetOreColor(cell.OreNode);
                    Gizmos.DrawCube(worldPos, new Vector3(CellSize * 0.6f, CellSize * 0.6f, 0));
                }
            }
        }
    }

    private Color GetOreColor(ResourceType type)
    {
        return type switch
        {
            ResourceType.Coal => Color.black,
            ResourceType.Iron => new Color(0.8f, 0.4f, 0.1f),
            ResourceType.Gold => Color.yellow,
            ResourceType.Diamond => Color.cyan,
            ResourceType.Quartz => Color.magenta,
            _ => Color.clear
        };
    }
}