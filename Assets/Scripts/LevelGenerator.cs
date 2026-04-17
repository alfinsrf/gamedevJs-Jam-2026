using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class LevelGenerator : MonoBehaviour
{
    [Header("Generation Settings")]    
    public int ClusterCount = 10;
    public int CoreProtectionRadius = 3;

    private GridManager _grid;
    private Vector2Int _corePosition;

    private void Start()
    {
        _grid = GridManager.Instance;
        _corePosition = new Vector2Int(_grid.Width / 2, _grid.Height / 2);

        // for testing
        PopulateResources(System.DateTime.Now.Millisecond);
    }

    public void PopulateResources(int seed)
    {
        Random.InitState(seed);

        for (int i = 0; i < ClusterCount; i++)
        {            
            ResourceType typeToSpawn = (ResourceType)Random.Range(1, 6);
            
            int clusterSize = Random.Range(2, 5);

            SpawnCluster(typeToSpawn, clusterSize);
        }
    }

    private void SpawnCluster(ResourceType type, int size)
    {
        int startX = Random.Range(0, _grid.Width);
        int startY = Random.Range(0, _grid.Height);

        int placed = 0;
        for (int x = startX; x < startX + 3 && placed < size; x++)
        {
            for (int y = startY; y < startY + 3 && placed < size; y++)
            {
                if (IsValidOrePlacement(x, y))
                {
                    _grid.GetCell(x, y).OreNode = type;
                    placed++;
                }
            }
        }
    }

    private bool IsValidOrePlacement(int x, int y)
    {
        // Bounds check
        if (_grid.GetCell(x, y) == null) return false;

        // Protection radius check
        float distToCore = Vector2Int.Distance(new Vector2Int(x, y), _corePosition);
        if (distToCore <= CoreProtectionRadius) return false;

        // Check if cell is already occupied
        if (_grid.GetCell(x, y).OreNode != ResourceType.None) return false;

        return true;
    }

    public void ClearLevel()
    {
        _grid.InitializeGrid();
    }
}