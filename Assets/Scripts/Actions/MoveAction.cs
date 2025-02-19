using System.Collections.Generic;
using UnityEngine;

public class MoveAction : GameAction
{
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int range = 2;

    private void OnEnable()
    {
        actionName = "Move"; // This will be the default name
    }

    public override bool CanExecute(Entity executor, Entity target)
    {
        Healer healer = executor as Healer;
        if (healer == null || !healer.HasActionPoints()) return false;

        // If we're just checking if the action is available (no target yet)
        if (target == null) return true;

        // If we have a target, check if it's a valid move
        Vector2Int targetPos = GridManager.Instance.WorldToGridPosition(target.transform.position);
        Vector2Int currentPos = executor.currentGridPosition;
        
        return Vector2Int.Distance(targetPos, currentPos) <= range 
            && GridManager.Instance.isValid(targetPos);
    }

    public override void Execute(Entity executor, Entity target)
    {
        Healer healer = executor as Healer;
        if (healer == null) return;

        MovementCursor cursor = FindFirstObjectByType<MovementCursor>();
        if (cursor != null && healer.TrySpendActionPoints(actionPointCost))
        {
            Vector2Int targetPos = cursor.GetCurrentPosition();
            Vector2Int direction = targetPos - healer.currentGridPosition;
            GridManager.Instance.moveEntity(healer, direction);
        }
    }

    public override Vector2Int[] GetValidTargetPositions(Entity executor)
    {
        Vector2Int currentPos = executor.currentGridPosition;
        List<Vector2Int> validPositions = new List<Vector2Int>();

        // Check all positions within range
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                // Skip if diagonal distance is greater than range
                if (Mathf.Abs(x) + Mathf.Abs(y) > range) continue; // This creates a diamond shape

                Vector2Int targetPos = currentPos + new Vector2Int(x, y);
                if (GridManager.Instance.isValid(targetPos))
                {
                    validPositions.Add(targetPos);
                }
            }
        }

        return validPositions.ToArray();
    }

    public override bool RequiresTarget(){
        return true;
    }
}