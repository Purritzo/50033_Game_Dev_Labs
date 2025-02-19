using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;
    private ActionContainer actionContainer;
    private Healer player;

    [SerializeField] private float actionDelay = 0.5f; // Delay between actions
    [SerializeField] private float moveSpeed = 0.3f; // Duration of movement animation

    void Start()
    {
        enemy = GetComponent<Enemy>();
        actionContainer = GetComponent<ActionContainer>();
        player = FindFirstObjectByType<Healer>();
    }

    public void ExecuteTurn()
    {
        StartCoroutine(ExecuteTurnSequence());
    }

    public IEnumerator ExecuteTurnSequence()
    {
        while (enemy.HasActionPoints())
        {
            // Check if we can attack
            if (IsAdjacentToPlayer())
            {
                var attackAction = actionContainer.GetAllActions().Find(a => a is EnemyAttackAction) as EnemyAttackAction;
                if (attackAction != null && attackAction.CanExecute(enemy, player))
                {
                    // Play attack animation
                    if (enemy.TryGetComponent<Animator>(out var animator))
                    {
                        animator.SetTrigger("Attack");
                        // Wait for animation event or fixed duration
                        yield return new WaitForSeconds(actionDelay);
                    }
                    
                    attackAction.Execute(enemy, player);
                    yield return new WaitForSeconds(actionDelay); // Add delay after attack
                    continue;
                }
            }

            // If we can't attack, try to move
            var moveAction = actionContainer.GetAllActions().Find(a => a is EnemyMoveAction) as EnemyMoveAction;
            if (moveAction != null && moveAction.CanExecute(enemy, null))
            {
                yield return StartCoroutine(ExecuteMoveAction(moveAction));
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(actionDelay); // Add delay between actions
        }
    }

    // public void ExecuteTurn()
    // {
    //     while (enemy.HasActionPoints())
    //     {
    //         Debug.Log(enemy.currentActionPoints);
    //         // Check if we can attack first
    //         if (IsAdjacentToPlayer())
    //         {
    //             var attackAction = actionContainer.GetAllActions().Find(a => a is EnemyAttackAction) as EnemyAttackAction;
    //             if (attackAction != null && attackAction.CanExecute(enemy, player))
    //             {
    //                 attackAction.Execute(enemy, player);
    //                 continue; // Try to attack again if we have AP
    //             }
    //         }

    //         // If we can't attack or don't have enough AP, try to move
    //         var moveAction = actionContainer.GetAllActions().Find(a => a is EnemyMoveAction) as EnemyMoveAction;
    //         if (moveAction != null && moveAction.CanExecute(enemy, null))
    //         {
    //             MoveTowardsPlayer(moveAction);
    //         }
    //         else
    //         {
    //             break; // No valid moves left
    //         }
    //     }
    // }

    private bool IsAdjacentToPlayer()
    {
        return Mathf.Abs(enemy.currentGridPosition.x - player.currentGridPosition.x) + 
               Mathf.Abs(enemy.currentGridPosition.y - player.currentGridPosition.y) <= 1;
    }

    private void MoveTowardsPlayer(EnemyMoveAction moveAction)
    {
        Vector2Int[] validPositions = moveAction.GetValidTargetPositions(enemy);
        
        // Find the valid position that gets us closest to the player
        Vector2Int bestPosition = enemy.currentGridPosition;
        float bestDistance = float.MaxValue;

        foreach (Vector2Int pos in validPositions)
        {
            float distance = Vector2.Distance(pos, player.currentGridPosition);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestPosition = pos;
            }
        }

        if (bestPosition != enemy.currentGridPosition)
        {
            // Calculate direction from current to best position
            Vector2Int moveDirection = bestPosition - enemy.currentGridPosition;
            GridManager.Instance.moveEntity(enemy, moveDirection); //this is not really direction ah, just the vector
            enemy.currentActionPoints -= 1; // for now...
            
        }
    }

    private Vector2Int FindBestMovePosition(Vector2Int[] validPositions)
    {
        Vector2Int bestPosition = enemy.currentGridPosition;
        float bestDistance = float.MaxValue;

        foreach (Vector2Int pos in validPositions)
        {
            float distance = Vector2.Distance(pos, player.currentGridPosition);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestPosition = pos;
            }
        }

        return bestPosition;
    }

    private IEnumerator ExecuteMoveAction(EnemyMoveAction moveAction)
    {
        Vector2Int[] validPositions = moveAction.GetValidTargetPositions(enemy);
        Vector2Int bestPosition = FindBestMovePosition(validPositions);

        if (bestPosition != enemy.currentGridPosition)
        {
            Vector2Int moveDirection = bestPosition - enemy.currentGridPosition;
            
            // Use GridManager's movement system
            yield return StartCoroutine(
                GridManager.Instance.MoveEntityWithAnimation(enemy, moveDirection)
            );
            
            enemy.currentActionPoints -= 1;
        }
    }


    // private IEnumerator ExecuteMoveAction(EnemyMoveAction moveAction)
    // {
    //     Vector2Int[] validPositions = moveAction.GetValidTargetPositions(enemy);
    //     Vector2Int bestPosition = FindBestMovePosition(validPositions);

    //     if (bestPosition != enemy.currentGridPosition)
    //     {
    //         // Start move animation
    //         if (enemy.TryGetComponent<Animator>(out var animator))
    //         {
    //             animator.SetBool("IsMoving", true);
    //         }

    //         Vector2Int moveDirection = bestPosition - enemy.currentGridPosition;
    //         Vector3 startPos = enemy.transform.position;
    //         Vector3 targetPos = GridManager.Instance.GridToWorldPosition(bestPosition);
            
    //         float elapsedTime = 0;
    //         while (elapsedTime < moveSpeed)
    //         {
    //             elapsedTime += Time.deltaTime;
    //             float t = elapsedTime / moveSpeed;
    //             enemy.transform.position = Vector3.Lerp(startPos, targetPos, t);
    //             yield return null;
    //         }

    //         // Update grid position after movement is complete
    //         GridManager.Instance.moveEntity(enemy, moveDirection);
    //         enemy.currentActionPoints -= 1;

    //         // End move animation
    //         if (enemy.TryGetComponent<Animator>(out animator))
    //         {
    //             animator.SetBool("IsMoving", false);
    //         }
    //     }
    // }
}