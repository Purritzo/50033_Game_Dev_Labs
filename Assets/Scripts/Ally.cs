using UnityEngine;

public class Ally : Entity
{
    public int attackPower = 10;
    public float attackInterval = 0.5f;
    private float timer = 0f;
    public Boss targetBoss;
    public Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        maxHealth = 100;
        health = maxHealth;
        hpBar.setMaxHealth(maxHealth);
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (timer >= attackInterval)
            {
                AttackBoss();
                timer = 0;
            } else 
            {
                timer += Time.deltaTime;
            }
        }
    }

    void AttackBoss()
    {
        targetBoss.TakeDamage(attackPower);
        Debug.Log(gameObject.name + " attacking Boss with " + attackPower);
    }

    public void ReceiveHeal(int IncomingHealValue)
    {
        int maximumCanHeal = maxHealth - health;
        if (IncomingHealValue > maximumCanHeal)
        {
            IncomingHealValue = maximumCanHeal;
        }
        health += IncomingHealValue;
        GameObject.FindWithTag("Manager").GetComponent<GameManager>().addScore(IncomingHealValue);
        Debug.Log(gameObject.name + " healed for " + IncomingHealValue);
        hpBar.SetHealth(health);
    }
}
