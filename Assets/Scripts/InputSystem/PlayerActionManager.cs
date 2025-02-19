using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerActionManager : MonoBehaviour
{

    public static PlayerActionManager Instance;
    public UnityEvent<Vector2Int> moveCheck;
    public UnityEvent confirmPress;
    public UnityEvent cancelPress;

    private void Awake()
    {
        Instance = this; // singleton things?
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnConfirmHoldAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("ConfirmHold was started");
        }
        else if (context.performed)
        {
            Debug.Log("ConfirmHold was performed");
        }
        else if (context.canceled)
        {
            //Debug.Log("ConfirmHold was cancelled");
        }
    }

    // called twice, when pressed and unpressed
    public void OnConfirmAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("Confirm was started");
        }
        else if (context.performed)
        {
            Debug.Log("Confirm was performed");
            confirmPress.Invoke();
        }
        else if (context.canceled)
        {
            //Debug.Log("Confirm was cancelled");
        }

    }

    // called twice, when pressed and unpressed
    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("move started");
            Vector2 move = context.ReadValue<Vector2>();
            Debug.Log($"move value: {move}"); // will return null when not pressed
            //int moveValue = move > 0 ? 1 : -1;
            //moveCheck.Invoke(moveValue);

            // Conversion into Vector2Int
            //Vector2Int direction = new(moveValue, 0);
            Vector2Int direction = Vector2Int.RoundToInt(move);
            Debug.Log(direction);
            moveCheck.Invoke(direction);
        }
        if (context.canceled)
        {
            //Debug.Log("move stopped");
            //moveCheck.Invoke(new(0,0));
        }
    }

    public void OnCancelPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cancelPress.Invoke();
        }
    }
}
