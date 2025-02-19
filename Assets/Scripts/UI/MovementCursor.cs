using UnityEngine;
using UnityEngine.UI;

public class MovementCursor : MonoBehaviour
{    
    [SerializeField] private Vector2Int currentGridPosition;
    private bool isActive = false;
    private GridManager gridManager;
    private Image cursorImage; // Change from SpriteRenderer to Image
    private RectTransform rectTransform;
    private Camera mainCamera;
    private Canvas parentCanvas;
    [SerializeField] private float sizeMultiplier = 1.1f;

    void Start()
    {
        gridManager = GridManager.Instance;
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
        gameObject.SetActive(false);

        // Calculate cursor size based on grid cell size in screen space
        Vector3 worldCellSize = gridManager.cellSize;
        Vector3 screenPoint1 = mainCamera.WorldToScreenPoint(Vector3.zero);
        Vector3 screenPoint2 = mainCamera.WorldToScreenPoint(new Vector3(worldCellSize.x, worldCellSize.y, 0));
        Vector2 screenSize = screenPoint2 - screenPoint1;

        // Ensure proper RectTransform setup
        rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Set the cursor size to match grid cell in screen space
        rectTransform.sizeDelta = screenSize;

        rectTransform.sizeDelta = screenSize * sizeMultiplier;

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // High number to ensure it's on top
        }

    }

    public void Show(Vector2Int startPosition)
    {
        currentGridPosition = startPosition;
        MoveTo(startPosition);
        //UpdateCursorPosition(startPosition);
        gameObject.SetActive(true);
        isActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    public void MoveTo(Vector2Int newGridPosition)
    {
        currentGridPosition = newGridPosition;
        Vector3 worldPos = gridManager.GridToWorldPosition(newGridPosition);
        Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        rectTransform.position = screenPos;
        // if (!isActive) return;
        // currentGridPosition = newPosition;
        // UpdateCursorPosition(newPosition);
    }

    private void UpdateCursorPosition(Vector2Int gridPosition)
    {
        Vector3 worldPosition = gridManager.GridToWorldPosition(gridPosition);
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Convert screen position to local canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.GetComponent<RectTransform>(),
            screenPosition,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out Vector2 localPoint
        );

        rectTransform.anchoredPosition = localPoint;
    }

    public Vector2Int GetCurrentPosition()
    {
        return currentGridPosition;
    }
}