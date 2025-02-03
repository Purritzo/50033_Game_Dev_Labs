using UnityEngine;

public class Boss : Entity
{
    public int attackPower = 30;
    public float attackInterval = 1f;
    private float timer = 0f;
    public Ally[] allies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        maxHealth = 300;
        health = maxHealth;
        hpBar.setMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= attackInterval)
        {
            Attack();
            timer = 0;
        } else 
        {
            timer += Time.deltaTime;
        }
    }

    void Attack()
    {
        Ally[] aliveAllies = System.Array.FindAll(allies, ally => ally.health > 0);

        // Ensure there's at least one alive ally before attacking
        if (aliveAllies.Length > 0)
        {
            Ally target = aliveAllies[Random.Range(0, aliveAllies.Length)];
            target.TakeDamage(attackPower);
            Debug.Log("Boss attacked " + target.name);
        }
        else
        {
            Debug.Log("No alive allies left, boss does not attack.");
        }

        // Ally target = allies[Random.Range(0, allies.Length)];
        // target.TakeDamage(attackPower);
        // //target.health -= attackPower;
        // Debug.Log("Boss attacked " + target.name);
    }

    public override void TakeDamage(int incomingAttackPower)
    {
        health -= incomingAttackPower;
        hpBar.SetHealth(health);
        //GameObject.FindWithTag("Manager").GetComponent<GameManager>().addScore(incomingAttackPower);
    }
}
