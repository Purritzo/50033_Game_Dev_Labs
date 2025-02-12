using System.Collections;
using UnityEngine;

public class Boss : Entity
{
    public int attackPower = 30;
    public int specialAttackPower = 60; // Special attack damage, MAKE SURE THIS HURTS TO PUNISH PLAYER BEING LAZY
    // USE PREFAB NEXT TIME
    //public GameObject telegraphPrefab; // Assign a red warning circle, uhh... see if FFXIV style is possible
    public float telegraphRadius = 3.0f; // Size of the telegraph
    public Transform attackPoint; // The position where the attack happens
    public LayerMask playerLayer; // Layer for detecting players
    public float attackInterval = 1f;
    private float timer = 0f;
    public Ally[] allies;
    public BasicExpandingTelegraph telegraphEffectPreFab;
    public Animator bossAnimator;
    public bool halfwayHPTriggerReady = true;
    public bool canAttack = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new public void Start()
    {
        base.Start();
        maxHealth = 600;
        health = maxHealth;
        hpBar.setMaxHealth(maxHealth);
        halfwayHPTriggerReady = true;
        canAttack = true;

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
        if (halfwayHPTriggerReady == true && ((float)health/maxHealth < 0.75)){
            SpecialAttack();
            halfwayHPTriggerReady = false;
        }
    }

    void Attack()
    {
        Ally[] aliveAllies = System.Array.FindAll(allies, ally => ally.health > 0);

        // Ensure there's at least one alive ally before attacking
        if (aliveAllies.Length > 0 && canAttack == true)
        {
            Ally target = aliveAllies[Random.Range(0, aliveAllies.Length)];
            bossAnimator.SetTrigger("Attack");
            target.TakeDamage(attackPower);
            //Debug.Log("Boss attacked " + target.name);
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

    void SpecialAttack()
    {
        bossAnimator.SetTrigger("SpecialAttack");
        BasicExpandingTelegraph telegraphEffect = Instantiate(telegraphEffectPreFab);
        telegraphEffect.StartWarning();
        canAttack = false;
    }

    // TODO: RESTRUCTURE AGAIN
    void DealSpecialAttackDamage()
    {
        // using hardcoded distance for now...
        Physics2D.queriesHitTriggers=true;
        Collider2D[] playersHit = Physics2D.OverlapCircleAll(transform.position, 2.5f, playerLayer);
        foreach (Collider2D player in playersHit)
        {
            // Assuming player has a script with a TakeDamage() method
            Debug.Log(player);
            Debug.Log(player.gameObject);
            if (player is BoxCollider2D)
            {
                player.gameObject.GetComponent<Ally>().TakeDamage(specialAttackPower);
            }
        }
        canAttack = true;
    }


    // public override void TakeDamage(int incomingAttackPower)
    // {
    //     health -= incomingAttackPower;
    //     hpBar.SetHealth(health);
    //     //GameObject.FindWithTag("Manager").GetComponent<GameManager>().addScore(incomingAttackPower);
    // }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}
