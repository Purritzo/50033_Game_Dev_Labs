using UnityEngine;

public class Entity : MonoBehaviour
{
    public string entityName;
    public int maxHealth;
    public int health;
    public HPBar hpBar;
    public bool incapcitated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(int incomingAttackPower)
    {
        health -= incomingAttackPower;
        health = Mathf.Max(health, 0); // Prevent negative health
        hpBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
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
