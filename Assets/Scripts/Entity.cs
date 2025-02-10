using UnityEngine;

public class Entity : MonoBehaviour
{
    public string entityName;
    public int maxHealth;
    public int health;
    public HPBar hpBar;
    public bool incapcitated;
    public GameObject floatingTextPrefab;

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
        int damage = Mathf.Min(health-0, incomingAttackPower);
        health -= incomingAttackPower;
        health = Mathf.Max(health, 0); // Prevent negative health
        ShowFloatingText(damage.ToString(), Color.red);
        hpBar.SetHealth(health);
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
