using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction : GameAction
{
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int attackPower = 20;
    [SerializeField] private int range = 1;

    private void OnEnable()
    {
        actionName = "Attack";
        description = "Attack an enemy in range";
    }

    public override bool CanExecute(Entity executor, Entity target)
    {
        Healer healer = executor as Healer;
        if (healer == null || !healer.HasActionPoints()) return false;

        // If we're just checking if the action is available (no target yet)
        if (target == null) return true;

        // Check if target is in range
        return Vector2Int.Distance(target.currentGridPosition, healer.currentGridPosition) <= range;
    }

    public override void Execute(Entity executor, Entity target)
    {
        Healer healer = executor as Healer;
        if (healer == null) return;

        if (healer.TrySpendActionPoints(actionPointCost))
        {
            if (target != null)
            {
                target.TakeDamage(attackPower);
                
                // Play animation
                if (healer.healerAnimator != null)
                {
                    healer.healerAnimator.SetTrigger("Attack");
                }
            }
        }
    }

    public override Vector2Int[] GetValidTargetPositions(Entity executor)
    {
        Vector2Int currentPos = executor.currentGridPosition;
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > range) continue;

                Vector2Int targetPos = currentPos + new Vector2Int(x, y);
                if (GridManager.Instance.IsValidAttackTarget(targetPos) && GridManager.Instance.grid[targetPos.x, targetPos.y] is Enemy)
                {
                    validPositions.Add(targetPos);
                }
            }
        }

        return validPositions.ToArray();
    }
    public override bool RequiresTarget()
    {
        return true;
    }
}