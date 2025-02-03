using System.Collections;
using UnityEngine;

public class Healer : Entity
{

    public int healAmount = 20;
    public int attackPower = 20;
    public Ally targetedAlly;
    public Ally[] allies;
    private bool currentlyTargetingSelf = false;
    private bool targetingBoss = false;
    public Vector3 startPosition;
    public TargetingManager targetingManager;
    public CastBar castBar;
    public Rigidbody2D playerBody;
    private Coroutine castingCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        castBar = gameObject.transform.Find("CastBarCanvas/CastBar").GetComponent<CastBar>();
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            targetedAlly = null;
            currentlyTargetingSelf = true;
            targetingManager.MoveIndicatorToEntity(FindFirstObjectByType<Healer>());
        }
        if (Input.GetKeyDown("w"))
        {
            targetedAlly = allies[0];
            targetingManager.MoveIndicatorToEntity(targetedAlly);
            currentlyTargetingSelf = false;
        }
        if (Input.GetKeyDown("e"))
        {
            targetedAlly = allies[1];
            targetingManager.MoveIndicatorToEntity(targetedAlly);
            currentlyTargetingSelf = false;
        }
        if (Input.GetKeyDown("f"))
        {
            if (targetedAlly != null)
            {
                if (castingCoroutine != null) // Stop previous cast if any
                {
                    StopCoroutine(castingCoroutine);
                    castBar.CancelCast();
                }
                castingCoroutine = StartCoroutine(CastSpell(0.5f));
                //StartCoroutine(CastSpell(0.5f));
                //Heal(targetedAlly);
            } else if (targetingBoss == true)
            {
                if (castingCoroutine != null) // Stop previous cast if any
                {
                    StopCoroutine(castingCoroutine);
                    castBar.CancelCast();
                }
                castingCoroutine = StartCoroutine(CastAttackSpell(1.0f));
            }
        }
        if (Input.GetKeyDown("r"))
        {
            targetedAlly = null;
            targetingBoss = true;
            targetingManager.MoveIndicatorToEntity(FindFirstObjectByType<Boss>());
        }

        // Interrupt cast on move
        if (playerBody.linearVelocity.magnitude > 0.1f)
        {
            if (castingCoroutine != null)
            {
                StopCoroutine(castingCoroutine);
                castBar.CancelCast();
            }
        }
    }

    void Heal(Ally target)
    {
        if (target.health < target.maxHealth)
        {
            // NOTE: Swapping targets just before the spell resolves heals the new target
            // Can be considered minor bug or a feature (When player level is high enough for mid-cast target swap)
            target.ReceiveHeal(healAmount);
            //target.health += healAmount;
            //Debug.Log(target.name + " healed for " + healAmount);
        }
    }

    void Attack(Boss boss)
    {
        boss.TakeDamage(attackPower);
    }

    IEnumerator CastSpell(float duration)
    {
        castBar.StartCasting(duration);
        yield return new WaitForSeconds(duration);

        if (targetedAlly != null) 
        {
            Heal(targetedAlly);
        }
    }

    IEnumerator CastAttackSpell(float duration)
    {
        castBar.StartCasting(duration);
        yield return new WaitForSeconds(duration);
        Attack(FindFirstObjectByType<Boss>());
    }

    // TODO: Add healer taking damage also, if needed
}
