using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
// This manager is meant to deal with the semi-turn based system order, lower level than the game manager
{

    public static LevelManager Instance; // singleton things?

    public UnityEvent playerTurnStart;
    public UnityEvent playerTurnEnd;
    public UnityEvent allyTurnStart;
    public UnityEvent allyTurnEnd;
    public UnityEvent enemyTurnStart;
    public UnityEvent enemyTurnEnd;
    public Healer healer;
    public Ally[] allies;
    private UIManager uiManager;

    private enum TurnState { Player, Allies, Enemies }
    private TurnState currentTurn;
    [SerializeField] private PhaseTransition phaseTransition;
    

    private void Awake()
    {
        Instance = this; // singleton things?
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        StartPlayerTurn();
        healer = FindFirstObjectByType<Healer>();
        // if (healer != null)
        // {
        //     healer.onPlayerAction.AddListener(NotifyPlayerActionTaken);
        // }
        //ActionManager.Instance.PlayerActionsDone.AddListener(StartAllyTurn);
        // ActionManager.Instance.AllyActionsDone.AddListener(EndAllyTurn);
        // ActionManager.Instance.EnemyActionsDone.AddListener(EndEnemyTurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPlayerTurn() => currentTurn == TurnState.Player;

    public void StartPlayerTurn()
    {
        StartCoroutine(StartPlayerTurnSequence());
    }

    private IEnumerator StartPlayerTurnSequence()
    {
        yield return StartCoroutine(phaseTransition.ShowPhaseTransition("Player Phase"));
        
        Debug.Log("Player's turn started.");
        currentTurn = TurnState.Player;
        healer.StartTurn();
        playerTurnStart?.Invoke();
    }

    // public void NotifyPlayerActionTaken()
    // {
    //     playerActionsRemaining--;

    //     if (playerActionsRemaining <= 0)
    //     {
    //         EndPlayerTurn();
    //         // Maybe confirmation menu instead
    //     }
    // }

    public void EndPlayerTurn()
    {
        Debug.Log("Player's turn ended.");
        playerTurnEnd?.Invoke();
        StartCoroutine(StartAllyTurnSequence());
        //StartAllyTurn();
    }

    // private void StartAllyTurn()
    // {
    //     Debug.Log("Ally turn started.");
    //     currentTurn = TurnState.Allies;
    //     allyTurnStart?.Invoke();
    //     // Whatever things for allies to do their stuff
    //     EndAllyTurn();
    //     //EndAllyTurn(); // ActionManager will call endallyturn
    // }

    private IEnumerator StartAllyTurnSequence()
    {
        yield return StartCoroutine(phaseTransition.ShowPhaseTransition("Ally Phase"));
        
        Debug.Log("Ally turn started.");
        currentTurn = TurnState.Allies;
        //allyTurnStart?.Invoke();
        //yield return StartCoroutine(ProcessAllyTurns()); //not yet implemented
        EndAllyTurn();
    }

    private void EndAllyTurn()
    {
        Debug.Log("Ally turn ended.");
        allyTurnEnd?.Invoke();
        StartCoroutine(StartEnemyTurnSequence());
        //StartEnemyTurn();
    }

    private IEnumerator StartEnemyTurnSequence()
    {
        yield return StartCoroutine(phaseTransition.ShowPhaseTransition("Enemy Phase"));
        
        Debug.Log("Enemy turn started.");
        currentTurn = TurnState.Enemies;
        enemyTurnStart?.Invoke();
        
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
             yield return StartCoroutine(ProcessEnemyTurn(enemy)); //ProcessEnemyTurn(enemy);
        }

        EndEnemyTurn();
    }

    // private void StartEnemyTurn()
    // {
    //     Debug.Log("Enemy turn started.");
    //     currentTurn = TurnState.Enemies;
    //     enemyTurnStart?.Invoke();
        
    //     // Process each enemy's turn
    //     Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    //     foreach (Enemy enemy in enemies)
    //     {
    //         ProcessEnemyTurn(enemy);
    //     }

    //     // End enemy turn after all enemies have acted
    //     EndEnemyTurn();
    // }

    private IEnumerator ProcessEnemyTurn(Enemy enemy)
    {
        enemy.StartTurn();
        var enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            yield return StartCoroutine(enemyAI.ExecuteTurnSequence());
        }
    }

    // private void ProcessEnemyTurn(Enemy enemy)
    // {
    //     enemy.StartTurn();
    //     var enemyAI = enemy.GetComponent<EnemyAI>();
    //     if (enemyAI != null)
    //     {
    //         enemyAI.ExecuteTurn();
    //     }
    //     // var actionContainer = enemy.GetComponent<ActionContainer>();
        
    //     // while (enemy.HasActionPoints())
    //     // {
    //     //     var availableActions = actionContainer.GetAvailableActions();
    //     //     if (availableActions.Count == 0) break;

    //     //     // For now, just take the first available action
    //     //     var action = availableActions[0];
    //     //     action.Execute(enemy, null);
    //     // }
    // }

    private void EndEnemyTurn()
    {
        Debug.Log("Enemy turn ended.");
        enemyTurnEnd?.Invoke();
        StartPlayerTurn(); // Loop back to the player's turn
    }
}
