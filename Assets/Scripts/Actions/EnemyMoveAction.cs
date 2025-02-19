using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAction : GameAction
{
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int range = 2;

    private void OnEnable()
    {
        actionName = "Move";
    }

    public override bool CanExecute(Entity executor, Entity target)
    {
        Enemy enemy = executor as Enemy;
        return enemy != null && enemy.HasActionPoints();
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
                if (GridManager.Instance.isValid(targetPos))
                {
                    validPositions.Add(targetPos);
                }
            }
        }

        return validPositions.ToArray();
    }

    public override void Execute(Entity executor, Entity target)
    {
        Enemy enemy = executor as Enemy;
        if (enemy == null) return;

        if (enemy.TrySpendActionPoints(actionPointCost))
        {
            //enemy.GetComponent<EnemyAI>().ExecuteTurn();
            // For now, move towards the player
            // Healer player = FindFirstObjectByType<Healer>();
            // Vector2Int playerPos = player.currentGridPosition;
            // Vector2Int[] validMoves = GetValidTargetPositions(enemy);

            // // Find the best move by selecting the tile that gets closest to the player
            // Vector2Int bestMove = enemy.currentGridPosition;
            // float bestDistance = Vector2Int.Distance(enemy.currentGridPosition, playerPos);

            // foreach (Vector2Int move in validMoves)
            // {
            //     float distance = Vector2Int.Distance(move, playerPos);
            //     if (distance < bestDistance)
            //     {
            //         bestDistance = distance;
            //         bestMove = move;
            //     }
            // }

            // // Move enemy to the best position
            // GridManager.Instance.moveEntity(enemy, bestMove - enemy.currentGridPosition);
            // Vector2Int direction = player.currentGridPosition - enemy.currentGridPosition;
            // // Find the best axis-aligned move (either horizontal or vertical)
            // // optimise this eventually, esp the "new" keywords
            // if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            // {
            //     direction = new Vector2Int(Mathf.Clamp(direction.x, -1, 1), 0); // Prioritize horizontal movement
            // }
            // else
            // {
            //     direction = new Vector2Int(0, Mathf.Clamp(direction.y, -1, 1)); // Prioritize vertical movement
            // }
            // GridManager.Instance.moveEntity(enemy, direction);
        }
    }

    public override bool RequiresTarget(){
        return true;
    }
}