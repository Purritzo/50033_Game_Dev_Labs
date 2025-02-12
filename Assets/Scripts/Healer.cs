using System.Collections;
using UnityEngine;

public class Healer : Entity
{

    public int healAmount = 20;
    public int attackPower = 20;
    public Ally targetedAlly;
    public Ally[] allies;
    public GameObject partyListContainer;
    private bool currentlyTargetingSelf = false;
    private bool targetingBoss = false;
    public Vector3 startPosition;
    public TargetingManager targetingManager;
    public CastBar castBar;
    public Rigidbody2D playerBody;
    private Coroutine castingCoroutine;
    public Animator healerAnimator;
    public AudioSource healerAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        maxHealth = 50;
        health = maxHealth;
        castBar = gameObject.transform.Find("CastBarCanvas/CastBar").GetComponent<CastBar>();
        startPosition = transform.localPosition;
        healerAnimator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        HandleTargeting();
        HandleHealOrAttackCast();

        // Interrupt cast on move
        if (playerBody.linearVelocity.magnitude > 0.1f)
        {
            if (castingCoroutine != null)
            {
                healerAnimator.SetTrigger("CastCancel");
                StopCoroutine(castingCoroutine);
                castBar.CancelCast();
            }
        }
    }

    void HandleHealOrAttackCast(){
        if (Input.GetKeyDown("f"))
        {
            if (targetedAlly != null && targetedAlly.health > 0)
            {
                if (castingCoroutine != null) // Stop previous cast if any
                {
                    healerAnimator.SetTrigger("CastCancel");
                    StopCoroutine(castingCoroutine);
                    castBar.CancelCast();
                }
                healerAnimator.SetTrigger("Casting");
                castingCoroutine = StartCoroutine(CastSpell(0.5f));
                //StartCoroutine(CastSpell(0.5f));
                //Heal(targetedAlly);
            } else if (targetingBoss == true)
            {
                if (castingCoroutine != null) // Stop previous cast if any
                {
                    healerAnimator.SetTrigger("CastCancel");
                    StopCoroutine(castingCoroutine);
                    castBar.CancelCast();
                }
                healerAnimator.SetTrigger("Casting");
                castingCoroutine = StartCoroutine(CastAttackSpell(1.0f));
            }
        }
    }

    void HandleTargeting(){
        if (Input.GetKeyDown("q"))
        {
            targetedAlly = null;
            currentlyTargetingSelf = true;
            targetingManager.MoveIndicatorToEntity(FindFirstObjectByType<Healer>());
            UpdatePartyUIHighlight();
        }
        if (Input.GetKeyDown("w"))
        {
            targetedAlly = allies[0];
            targetingManager.MoveIndicatorToEntity(targetedAlly);
            currentlyTargetingSelf = false;
            UpdatePartyUIHighlight();
        }
        if (Input.GetKeyDown("e"))
        {
            targetedAlly = allies[1];
            targetingManager.MoveIndicatorToEntity(targetedAlly);
            currentlyTargetingSelf = false;
            UpdatePartyUIHighlight();
        }
        if (Input.GetKeyDown("r"))
        {
            targetedAlly = null;
            targetingBoss = true;
            targetingManager.MoveIndicatorToEntity(FindFirstObjectByType<Boss>());
            UpdatePartyUIHighlight();
        }
    }

    void SetTarget(Ally newTarget)
    {
        targetedAlly = newTarget;
        UpdatePartyUIHighlight();
    }

    void UpdatePartyUIHighlight()
    {
        foreach (PartyMember partyMember in partyListContainer.GetComponentsInChildren<PartyMember>())
        {
            //Debug.Log(partyMember);
            //Debug.Log(partyMember.entity == targetedAlly);
            if (partyMember != null)
            {
                partyMember.SetHighlight(partyMember.entity == targetedAlly);
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

    void playHealSound(){
        healerAudio.PlayOneShot(healerAudio.clip);
    }

    void Attack(Boss boss)
    {
        boss.TakeDamage(attackPower);
    }

    IEnumerator CastSpell(float duration)
    {
        castBar.StartCasting(duration);
        yield return new WaitForSeconds(duration);
        healerAnimator.SetTrigger("CastSuccess");
        castingCoroutine = null;

        if (targetedAlly != null) 
        {
            Heal(targetedAlly);
        }
    }

    IEnumerator CastAttackSpell(float duration)
    {
        castBar.StartCasting(duration);
        yield return new WaitForSeconds(duration);
        healerAnimator.SetTrigger("CastSuccess");
        Attack(FindFirstObjectByType<Boss>());
        castingCoroutine = null;
    }

    // TODO: Add healer taking damage also, if needed
}
