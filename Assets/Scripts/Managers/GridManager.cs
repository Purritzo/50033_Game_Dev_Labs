using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
// This manager keeps track of where every entity is at, validates the movements and moves the entities
{

    public static GridManager Instance;
    public Tilemap groundTilemap;  // Assign in Inspector
    public Tilemap obstacleTilemap; // Assign if needed
    public Vector3 cellSize;
    public Entity[,] grid; // Tracking of where is where
    public int gridWidth, gridHeight;

    [SerializeField] private GameObject highlightTilePrefab; // Assign in inspector
    private List<GameObject> currentHighlights = new List<GameObject>();
    [SerializeField] private float defaultMoveDuration = 0.2f;
    private void Awake()
    {
        Instance = this;
        cellSize = groundTilemap.layoutGrid.cellSize;
        grid = new Entity[gridWidth, gridHeight]; // Prepare 2D array to track
        // THIS IS PLACED HERE BECAUSE IF ITS IN START SCRIPT ORDER MATTERS
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cellSize = FindFirstObjectByType<Grid>().cellSize; // Get Cellsize from existing Grid gameobject in scene
        //grid = new Entity[gridWidth, gridHeight]; // Prepare 2D array to track
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return (Vector2Int)groundTilemap.WorldToCell(worldPosition);
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        // Add origin if not (0, 0) grid position is in middle of tilemap
        //Debug.Log(groundTilemap.origin);
        Vector3Int tileMapOrigin = groundTilemap.origin;
        Vector3Int gridOffset = new(1, 1, 0);
        Vector3 tileMapCell = groundTilemap.GetCellCenterWorld((Vector3Int)gridPosition + gridOffset);
        Debug.Log(gridPosition + " grid converted to " + (tileMapCell + groundTilemap.origin));
        return tileMapCell + groundTilemap.origin;
    }

    public void RegisterEntityPosition(Entity entity, Vector2Int position)
    {
        if (isValid(position))
        {
            grid[position.x, position.y] = entity;
            entity.currentGridPosition = position;
        }
        else
        {
            Debug.LogError($"Invalid starting position {position} for entity {entity.name}");
        }
    }
    public bool isValid(Vector2Int newPosition)
    {
        // Check if the position is within grid bounds
        if (newPosition.x < 0 || newPosition.x >= grid.GetLength(0) || 
            newPosition.y < 0 || newPosition.y >= grid.GetLength(1))
        {
            return false; // Out of bounds
        }

        // Check if another entity is already occupying the space
        // Be careful of initial placement collisions, to NOTE
        if (grid[newPosition.x, newPosition.y] != null)
        {
            Debug.LogWarning($"Position {newPosition} is already occupied by {grid[newPosition.x, newPosition.y].name}");
            return false; // Space is occupied
        }
        return true; // Valid move
    }

    public bool IsValidAttackTarget(Vector2Int position)
    {
        if (position.x < 0 || position.x >= gridWidth || 
            position.y < 0 || position.y >= gridHeight)
        {
            return false; // Out of bounds
        }

        return grid[position.x, position.y] is Enemy;
    }

    // public void moveEntity(Entity entity, Vector2Int direction)
    // {
    //     Debug.Log("Trying to move entity in GridManager");
    //     Vector2Int newGridPosition = entity.currentGridPosition + direction;
    //     if (isValid(newGridPosition))
    //     {
    //         Debug.Log("Valid new position");
    //         grid[entity.currentGridPosition.x, entity.currentGridPosition.y] = null;
    //         grid[newGridPosition.x, newGridPosition.y] = entity;
    //         entity.currentGridPosition = newGridPosition;
    //         StartCoroutine(MoveEntitySmooth(entity, GridToWorldPosition(newGridPosition)));
    //         //entity.transform.position = GridToWorldPosition(newGridPosition); // Direct move, to change to couroutine
    //     }
    // }

    public void moveEntity(Entity entity, Vector2Int direction)
    {
        StartCoroutine(MoveEntityWithAnimation(entity, direction));
    }

    // Coroutine for smooth movement
    private IEnumerator MoveEntitySmooth(Entity entity, Vector3 targetPosition)
    {
        float duration = 0.2f; // Movement speed
        float elapsed = 0f;
        Vector3 start = entity.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            entity.transform.position = Vector3.Lerp(start, targetPosition, elapsed / duration);
            yield return null;
        }

        entity.transform.position = targetPosition;
    }

    public void HighlightValidPositions(Vector2Int[] positions)
    {
        ClearHighlights();
        
        foreach (var position in positions)
        {
            if (position.x >= 0 && position.x < gridWidth && 
                position.y >= 0 && position.y < gridHeight)
            {
                Vector3 worldPos = GridToWorldPosition(position);
                GameObject highlight = Instantiate(highlightTilePrefab, worldPos, Quaternion.identity);
                currentHighlights.Add(highlight);
            }
        }
    }

    public void ClearHighlights()
    {
        foreach (var highlight in currentHighlights)
        {
            Destroy(highlight);
        }
        currentHighlights.Clear();
    }

    public Entity GetEntityAtPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < gridWidth && 
            position.y >= 0 && position.y < gridHeight)
        {
            return grid[position.x, position.y];
        }
        return null;
    }

    // Return a coroutine that can be yielded on
    public IEnumerator MoveEntityWithAnimation(Entity entity, Vector2Int direction)
    {
        Vector2Int newGridPosition = entity.currentGridPosition + direction;
        if (isValid(newGridPosition))
        {
            // Update grid state
            grid[entity.currentGridPosition.x, entity.currentGridPosition.y] = null;
            grid[newGridPosition.x, newGridPosition.y] = entity;
            entity.currentGridPosition = newGridPosition;

            // Animate movement
            Vector3 targetPosition = GridToWorldPosition(newGridPosition);
            yield return StartCoroutine(AnimateMovement(entity, targetPosition));

            // Notify any listeners that movement is complete
            //entity.OnMovementComplete?.Invoke();
        }
    }

    private IEnumerator AnimateMovement(Entity entity, Vector3 targetPosition, float duration = -1)
    {
        if (duration < 0) duration = defaultMoveDuration;
        
        float elapsed = 0f;
        Vector3 startPosition = entity.transform.position;

        // Start movement animation if entity has an animator
        if (entity.TryGetComponent<Animator>(out var animator))
        {
            animator.SetBool("IsMoving", true);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            entity.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // Ensure final position is exact
        entity.transform.position = targetPosition;

        // End movement animation
        if (entity.TryGetComponent<Animator>(out animator))
        {
            animator.SetBool("IsMoving", false);
        }
    }
    
    // Not yet implementing this
    // public bool IsTileWalkable(Vector2Int gridPosition)
    // {
    //     return groundTilemap.HasTile((Vector3Int)gridPosition) && 
    //            !obstacleTilemap.HasTile((Vector3Int)gridPosition);
    // }
        public Vector2Int GetRandomEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        // Collect all empty positions
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (grid[x, y] == null)
                {
                    emptyPositions.Add(pos);
                }
            }
        }

        if (emptyPositions.Count == 0)
        {
            Debug.LogWarning("No empty positions found in grid!");
            return Vector2Int.zero; // Or handle this case as needed
        }

        // Return a random position from the list
        int randomIndex = Random.Range(0, emptyPositions.Count);
        return emptyPositions[randomIndex];
    }

    // Optional: Get random position with constraints
    public Vector2Int GetRandomEmptyPosition(int minDistanceFromPlayer)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();
        Healer player = FindFirstObjectByType<Healer>();
        Vector2Int playerPos = player.currentGridPosition;

        // Collect all empty positions that meet the distance requirement
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (grid[x, y] == null && 
                    Vector2Int.Distance(pos, playerPos) >= minDistanceFromPlayer)
                {
                    validPositions.Add(pos);
                }
            }
        }

        if (validPositions.Count == 0)
        {
            Debug.LogWarning("No valid positions found with given constraints!");
            return GetRandomEmptyPosition(); // Fall back to any empty position
        }

        int randomIndex = Random.Range(0, validPositions.Count);
        return validPositions[randomIndex];
    }
}
