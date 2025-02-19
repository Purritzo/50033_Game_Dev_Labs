using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAction : GameAction
{
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int attackPowerMultiplier = 1;
    private int attackPower;
    [SerializeField] private int range = 1;

    private void OnEnable()
    {
        actionName = "Attack";
    }

    public override bool CanExecute(Entity executor, Entity target)
    {
        Enemy enemy = executor as Enemy;
        if (enemy == null || !enemy.HasActionPoints()) return false;

        // Check if player is in range
        Healer player = FindFirstObjectByType<Healer>();
        return Vector2Int.Distance(player.currentGridPosition, enemy.currentGridPosition) <= range;
    }

    public override void Execute(Entity executor, Entity target)
    {
        Enemy enemy = executor as Enemy;
        if (enemy == null) return;

        if (enemy.TrySpendActionPoints(actionPointCost))
        {
            Healer player = FindFirstObjectByType<Healer>();

            // Start attack animation
            // if (enemy.TryGetComponent<Animator>(out var animator))
            // {
            //     animator.SetTrigger("Attack");
            // }

            // Play attack sound if available
            if (enemy.TryGetComponent<AudioSource>(out var audioSource))
            {
                audioSource.Play();
            }
            attackPower = attackPowerMultiplier * executor.GetComponent<Enemy>().attackPower;
            player.TakeDamage(attackPower);
            Debug.Log("attacking " + target + " for " + attackPower);
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
                if (GridManager.Instance.IsValidAttackTarget(targetPos) && GridManager.Instance.grid[targetPos.x, targetPos.y] is Healer) // add ally someday
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