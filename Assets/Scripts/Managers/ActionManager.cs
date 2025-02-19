using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionManager : MonoBehaviour
// This manager is to store all queued actions from allies and enemies and to execute them when its their turn
// Healer actions probably resolve here too actually
{

    public static ActionManager Instance;
    public UnityEvent PlayerActionsDone;
    public UnityEvent AllyActionsDone;
    public UnityEvent EnemyActionsDone;
    
    private Queue<ActionExecution> playerActions = new Queue<ActionExecution>();
    private Queue<ActionExecution> allyActions = new Queue<ActionExecution>();
    private Queue<ActionExecution> enemyActions = new Queue<ActionExecution>();
    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //LevelManager.Instance.playerTurnEnd.AddListener(ExecutePlayerActions);
        // LevelManager.Instance.allyTurnStart.AddListener(ExecuteAllyActions);
        // LevelManager.Instance.enemyTurnStart.AddListener(ExecuteEnemyActions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void QueuePlayerAction(ActionExecution action)
    {
        playerActions.Enqueue(action);
    }
    public void QueueAllyAction(ActionExecution action)
    {
        allyActions.Enqueue(action);
    }

    public void QueueEnemyAction(ActionExecution action)
    {
        enemyActions.Enqueue(action);
    }

    private void ExecutePlayerActions()
    {
        StartCoroutine(ExecuteActions(playerActions, PlayerActionsDone));
    }
    private void ExecuteAllyActions()
    {
        StartCoroutine(ExecuteActions(allyActions, AllyActionsDone));
    }

    private void ExecuteEnemyActions()
    {
        StartCoroutine(ExecuteActions(enemyActions, EnemyActionsDone));
    }

    private System.Collections.IEnumerator ExecuteActions(Queue<ActionExecution> actions, UnityEvent turnEndEvent)
    {
        while (actions.Count > 0)
        {
            var execution = actions.Dequeue();
            if (execution.Action.CanExecute(execution.Executor, execution.Target))
            {
                execution.Action.Execute(execution.Executor, execution.Target);
                yield return new WaitForSeconds(0.5f); // Animation time
            }
        }
        
        turnEndEvent?.Invoke(); // Signal the turn is over
    }
}
