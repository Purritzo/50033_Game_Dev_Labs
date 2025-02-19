using UnityEngine;
using UnityEngine.Events;

public class Enemy : Entity
{
    [SerializeField] private int maxActionPoints = 2;
    public int currentActionPoints;
    [SerializeField] public int attackPower = 20;
    [SerializeField] private int attackRange = 1;
    public UnityEvent enemyKilled;

    new void Start()
    {
        base.Start();
        maxHealth = 10;
        health = maxHealth;
        //hpBar.setMaxHealth(maxHealth);
        InitializeActions();
    }

    private void InitializeActions()
    {
        var actionContainer = GetComponent<ActionContainer>();
        if (actionContainer != null)
        {
            // Add basic enemy actions
            actionContainer.AddAction(ScriptableObject.CreateInstance<EnemyMoveAction>());
            actionContainer.AddAction(ScriptableObject.CreateInstance<EnemyAttackAction>());
        }
    }

    public void StartTurn()
    {
        currentActionPoints = maxActionPoints;
    }

    public bool HasActionPoints() => currentActionPoints > 0;

    public bool TrySpendActionPoints(int cost)
    {
        if (currentActionPoints >= cost)
        {
            currentActionPoints -= cost;
            return true;
        }
        return false;
    }

    protected override void Die()
    {
        Debug.Log(entityName + " has died.");
        enemyKilled.Invoke();
        GameManager.Instance.addScore(scoreValue);
        //Destroy(gameObject);
        // for now just teleport this guy somewhere...
        // sometimes doesn't work?
        Vector2Int newPos = gridManager.GetRandomEmptyPosition(3);

        //transform.position = gridManager.GridToWorldPosition(newPos);
        gridManager.moveEntity(this, newPos);

    }
}