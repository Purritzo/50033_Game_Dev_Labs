using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private GameObject actionButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject menuPanel;

    [Header("Layout Settings")]
    [SerializeField] private float buttonSpacing = 10f;
    [SerializeField] private Vector2 buttonSize = new Vector2(160, 40);
    
    private List<GameAction> currentActions = new List<GameAction>();
    private Entity selectedTarget;
    private GameAction selectedAction;
    [SerializeField] private GameObject movementCursorPrefab;
    private MovementCursor movementCursor;
    private bool isSelectingTarget = false;
    private Vector2Int[] validPositions;

    private void SetupMenuLayout()
    {
        // Setup the button container
        var layout = buttonContainer.GetComponent<HorizontalLayoutGroup>();
        if (layout == null) layout = buttonContainer.gameObject.AddComponent<HorizontalLayoutGroup>();
        
        layout.spacing = buttonSpacing;
        layout.padding = new RectOffset(10, 10, 10, 10);
        layout.childAlignment = TextAnchor.UpperCenter;
        
        // Setup the menu panel background
        var menuRect = menuPanel.GetComponent<RectTransform>();
        menuRect.anchorMin = new Vector2(0, 0);
        menuRect.anchorMax = new Vector2(1, 1);
        menuRect.pivot = new Vector2(0.5f, 0.5f);
    }

    private void Start()
    {
        // Ensure menu is hidden at start
        PlayerActionManager.Instance.cancelPress.AddListener(HandleCancel);
        GameObject cursorObj = Instantiate(movementCursorPrefab, transform);
        movementCursor = cursorObj.GetComponent<MovementCursor>();
        Hide();
    }

    public void Show()
    {
        menuPanel.SetActive(true);
    }

    public void Hide()
    {
        menuPanel.SetActive(false);
        ClearButtons();
        GridManager.Instance.ClearHighlights();
    }

    public void ShowActions(List<GameAction> availableActions)
    {
        if (!LevelManager.Instance.IsPlayerTurn()) return; // redudant but just putting this here in case

        currentActions = availableActions;
        ClearButtons();
        
        foreach (var action in availableActions)
        {
            var buttonObj = Instantiate(actionButtonPrefab, buttonContainer);
            var button = buttonObj.GetComponent<ActionButton>();
            var buttonComponent = buttonObj.GetComponent<Button>();
            button.actionName = action.GetName(); // Update ActionButton display


            // Check if action is executable
            bool canExecute = action.CanExecute(LevelManager.Instance.healer, null);
            
            // Set button interactability and visual state
            buttonComponent.interactable = canExecute;
            CanvasGroup buttonCanvasGroup = buttonObj.GetComponent<CanvasGroup>();
            if (buttonCanvasGroup != null)
            {
                buttonCanvasGroup.alpha = canExecute ? 1f : 0.5f;
            }
            
            buttonComponent.onClick.AddListener(() => OnActionSelected(action));

            //button.Setup(action.GetName(), () => OnActionSelected(action));
        }

        menuCanvasGroup.alpha = 1f;
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
        Debug.Log("Showing Panel");
        Show();
    }


    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnActionSelected(GameAction action)
    {
        selectedAction = action;
        // Show valid target positions
        validPositions = action.GetValidTargetPositions(LevelManager.Instance.healer);
        if (validPositions == null)
        {
            Debug.LogError("Valid positions array is null!");
            return;
        }
        Debug.Log($"Number of valid positions: {validPositions.Length}");
        GridManager.Instance.HighlightValidPositions(validPositions);
        isSelectingTarget = true;

        // Dim the menu and disable interaction
        menuCanvasGroup.alpha = 0.5f;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        //Hide(); // This is like... to give focus on the map, maybe change this to highlight the action selected instead
        // Remember this clears highlights

        Healer healer = LevelManager.Instance.healer;
        movementCursor.Show(healer.currentGridPosition);
        
        // Subscribe to input events
        PlayerActionManager.Instance.moveCheck.AddListener(HandleCursorMovement);
        PlayerActionManager.Instance.confirmPress.AddListener(HandleTargetConfirmation);
        // else
        // {
        //     // Maybe change to not execute instantly for those with no targets....
        //     ExecuteAction(null);
        // }
    }

    private void HandleCursorMovement(Vector2Int direction)
    {
        if (!isSelectingTarget || validPositions == null) return;

        Vector2Int newPosition = movementCursor.GetCurrentPosition() + direction;
        Debug.Log($"Attempting to move to position: {newPosition}");
        
        // Check if the new position is among valid positions
        if (validPositions.Contains(newPosition))
        {
            movementCursor.MoveTo(newPosition);
        }
    }

    private void HandleTargetConfirmation()
    {
        if (!isSelectingTarget) return;

        Vector2Int targetPos = movementCursor.GetCurrentPosition();
        Healer healer = LevelManager.Instance.healer;

        if (validPositions.Contains(targetPos))
        {
            Entity targetEntity = null;

            if (selectedAction is MoveAction)
            {
                // For movement, we just use the position
                Vector2Int moveDirection = targetPos - healer.currentGridPosition;
                selectedAction.Execute(healer, null);
            }
            else
            {
                // For targeting actions (attack, heal, etc)
                targetEntity = GridManager.Instance.grid[targetPos.x, targetPos.y];
                if (targetEntity != null && selectedAction.CanExecute(healer, targetEntity))
                {
                    selectedAction.Execute(healer, targetEntity);
                }
            }
            
            ActionManager.Instance.QueuePlayerAction(new ActionExecution(
                selectedAction,
                healer,
                targetEntity
            ));

            // Clean up
            CleanupTargeting();
            RestoreMenu();
            RefreshActionButtons();
        }
    }

    private void CleanupTargeting()
    {
        isSelectingTarget = false;
        movementCursor.Hide();
        PlayerActionManager.Instance.moveCheck.RemoveListener(HandleCursorMovement);
        PlayerActionManager.Instance.confirmPress.RemoveListener(HandleTargetConfirmation);
        GridManager.Instance.ClearHighlights();
    }

    private void HandleTargetSelection(Vector2Int direction)
    {
        if (!isSelectingTarget) return;
        
        Healer healer = LevelManager.Instance.healer;
        Vector2Int targetPos = healer.currentGridPosition + direction;

        if (selectedAction.CanExecute(healer, null) && 
            GridManager.Instance.isValid(targetPos))
        {
            // Execute the move
            selectedAction.Execute(healer, null);
            
            // Queue the action for the turn system
            ActionManager.Instance.QueuePlayerAction(new ActionExecution(
                selectedAction,
                healer,
                null
            ));

            // Clean up
            isSelectingTarget = false;
            PlayerActionManager.Instance.moveCheck.RemoveListener(HandleTargetSelection);
            GridManager.Instance.ClearHighlights();
            RestoreMenu();
        }
    }


    private void RefreshActionButtons()
    {
        // Refresh all action buttons' states
        foreach (Transform child in buttonContainer)
        {
            var button = child.GetComponent<ActionButton>();
            var buttonComponent = child.GetComponent<Button>();
            var buttonCanvasGroup = child.GetComponent<CanvasGroup>();
            
            if (button != null)
            {
                // Find the corresponding action
                var action = currentActions.Find(a => a.GetName() == button.actionName);
                if (action != null)
                {
                    bool canExecute = action.CanExecute(LevelManager.Instance.healer, null);
                    buttonComponent.interactable = canExecute;
                    if (buttonCanvasGroup != null)
                    {
                        buttonCanvasGroup.alpha = canExecute ? 1f : 0.5f;
                    }
                }
            }
        }
    }

    private void HandleCancel()
    {
        if (!isSelectingTarget) return;

        // Reset everything
        CleanupTargeting();
        RestoreMenu();
    }

    private void RestoreMenu(){
        // Re-enable menu
        menuCanvasGroup.alpha = 1f;
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
    }

    public void OnGridPositionSelected(Vector2Int gridPos)
    {
        if (selectedAction == null) return;
        
        Entity targetEntity = GridManager.Instance.GetEntityAtPosition(gridPos);
        if (selectedAction.CanExecute(LevelManager.Instance.healer, targetEntity))
        {
            ExecuteAction(targetEntity);
        }
    }

    private void ExecuteAction(Entity target)
    {
        selectedAction.Execute(LevelManager.Instance.healer, target);
        // ActionManager.Instance.QueuePlayerAction(new ActionExecution(
        //     selectedAction,
        //     LevelManager.Instance.healer,
        //     target)
        // );
        
        // No hiding yet, maybe if no AP?
        //Hide();
    }
}