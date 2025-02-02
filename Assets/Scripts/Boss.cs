using UnityEngine;

public class Boss : MonoBehaviour
{

    public int maxHealth = 300;
    public int health;
    public int attackPower = 30;
    public float attackInterval = 1f;
    private float timer = 0f;
    public Ally[] allies;
    public HPBar hpBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        Ally target = allies[Random.Range(0, allies.Length)];
        target.TakeDamage(attackPower);
        //target.health -= attackPower;
        Debug.Log("Boss attacked " + target.name);
    }

    public void TakeDamage(int incomingAttackPower)
    {
        health -= incomingAttackPower;
        hpBar.SetHealth(health);
    }
}
