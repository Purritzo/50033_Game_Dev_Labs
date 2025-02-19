using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public string entityName;
    public int maxHealth;
    public int health;
    public HPBar hpBar;
    public bool incapcitated;
    public GameObject floatingTextPrefab;
    public UnityEvent<Entity, Vector2Int> requestMove;
    [Header("Grid Settings")]
    [SerializeField] private Vector2Int startingGridPosition;
    public Vector2Int currentGridPosition;
    public float gridSize = 1f;
    public GridManager gridManager;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>(); // There should only be 1 GridManager active
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // No z rotation

        // Initialize position
        currentGridPosition = startingGridPosition;
        transform.position = gridManager.GridToWorldPosition(startingGridPosition);
        
        // Register entity with grid manager
        gridManager.RegisterEntityPosition(this, startingGridPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public virtual void Move(Vector2Int direction)
    // {
    //     requestMove.Invoke(this, direction);
    // }

    public virtual void TakeDamage(int incomingAttackPower)
    {
        int damage = Mathf.Min(health-0, incomingAttackPower);
        health -= incomingAttackPower;
        health = Mathf.Max(health, 0); // Prevent negative health
        ShowFloatingText(damage.ToString(), Color.red);
        //hpBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void ReceiveHeal(int IncomingHealValue)
    {
        if (health > 0)
        {
            int maximumCanHeal = maxHealth - health;
            if (IncomingHealValue > maximumCanHeal)
            {
                IncomingHealValue = maximumCanHeal;
            }
            health += IncomingHealValue;
            //GameObject.FindWithTag("Manager").GetComponent<GameManager>().addScore(IncomingHealValue);
            Debug.Log(gameObject.name + " healed for " + IncomingHealValue);
            hpBar.SetHealth(health);
            ShowFloatingText("+" + IncomingHealValue, Color.green);
        }
    }

    void ShowFloatingText(string text, Color color)
    {
        if (floatingTextPrefab)
        {
            GameObject floatText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            floatText.GetComponent<FloatingText>().Setup(text, color);
        }
    }

    // Called when health reaches zero
    protected virtual void Die()
    {
        incapcitated = true;
        Debug.Log(entityName + " has died.");
        // Destroy(gameObject);
    }

}
