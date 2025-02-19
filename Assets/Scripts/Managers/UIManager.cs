using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
// This manager ensures the menu for the player flows properly
{
    [Header("Canvas References")]
    [SerializeField] private Canvas mainCanvas;
    
    [Header("UI Sections")]
    [SerializeField] private RectTransform hudContainer;
    [SerializeField] private RectTransform actionMenusContainer;
    
    [Header("UI/Menu References")]
    [SerializeField] private PlayerActionMenu actionMenu;
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private PartyList partyList;

    [SerializeField] private GameObject cursorPrefab;

    
    private GameObject cursor;
    private bool isSelectingTarget;
    private bool isMenuOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursor = Instantiate(cursorPrefab);
        cursor.SetActive(false);

        if (actionMenu == null)
        {
            actionMenu = FindFirstObjectByType<PlayerActionMenu>();
        }
        actionMenu.Hide();
        
        PlayerActionManager.Instance.confirmPress.AddListener(HandleConfirmPress);
        LevelManager.Instance.playerTurnStart.AddListener(ActivatePlayerTurn);
        LevelManager.Instance.healer.UpdateActionPointsDisplay.AddListener(UpdateActionPointsDisplay);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectingTarget)
        {
            UpdateCursorPosition();
        }
    }

    private void SetupUIHierarchy()
    {
        // Setup containers with proper anchoring
        hudContainer.anchorMin = Vector2.zero;
        hudContainer.anchorMax = Vector2.one;
        
        // Position action menu container in bottom right
        actionMenusContainer.anchorMin = new Vector2(0.7f, 0);
        actionMenusContainer.anchorMax = Vector2.one;
    }

    public void ActivatePlayerTurn()
    // This should be called when PlayerTurnStart event is invoked
    {
        ShowPlayerActions();
    }

    public void ShowPlayerActions()
    {
        Debug.Log("ShowPlayerActions start");
        var healer = LevelManager.Instance.healer;
        var actionContainer = healer.GetComponent<ActionContainer>();
        if (actionContainer != null)
        {
            endTurnButton.SetActive(true);
            UpdateActionPointsDisplay();
            // var availableActions = actionContainer.GetAvailableActions();
            // Debug.Log("Getting action menu to show up");
            // actionMenu.ShowActions(availableActions);

            var allActions = actionContainer.GetAllActions();
            actionMenu.ShowActions(allActions);
        }
    }
    public void HandleConfirmPress()
    {
        if (!LevelManager.Instance.IsPlayerTurn()) return;
        // Debug.Log("Toggling Action Menu.");
        // ToggleActionMenu();
    }

    private void ToggleActionMenu()
    {
        isMenuOpen = !isMenuOpen;
        if (isMenuOpen)
        {
            ShowPlayerActions();
        }
        else
        {
            actionMenu.Hide();
            endTurnButton.SetActive(false);
        }
    }


    private void UpdateCursorPosition()
    {
        // Update cursor position based on input
        // When confirmed, call actionMenu.OnGridPositionSelected()
    }

    public void UpdateActionPointsDisplay(int current, int max)
    {
        actionPointsText.text = $"AP: {current}/{max}";
    }
    public void UpdateActionPointsDisplay()
    {
        var ap = LevelManager.Instance.healer.currentActionPoints;
        var maxAp = LevelManager.Instance.healer.maxActionPoints;
        actionPointsText.text = $"AP: {ap}/{maxAp}";
    }

    public void OnEndTurnClicked()
    {
        if (LevelManager.Instance.IsPlayerTurn())
        {
            endTurnButton.SetActive(false);
            actionMenu.Hide();
            LevelManager.Instance.EndPlayerTurn();
        }
    }
}
