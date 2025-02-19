using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Healer : Entity
{

    [SerializeField] public int maxActionPoints = 2;
    public int currentActionPoints;
    
    [SerializeField] public int maxMana = 100;
    [SerializeField] public int mana = 100;
    [SerializeField] private int healAmount = 20;
    [SerializeField] private int attackPower = 20;

    private List<GameAction> availableActions = new List<GameAction>();
    public Vector3 startPosition;
    public TargetingManager targetingManager;
    public CastBar castBar;
    public Rigidbody2D playerBody;
    private Coroutine castingCoroutine;
    public Animator healerAnimator;
    public AudioSource healerAudio;
    public UnityEvent onPlayerAction; // Event to notify LevelManager
    public UnityEvent<int, int> UpdateActionPointsDisplay;
    public ActionContainer actionContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        maxHealth = 50;
        health = maxHealth;
        castBar = gameObject.transform.Find("CastBarCanvas/CastBar").GetComponent<CastBar>();
        startPosition = transform.localPosition;
        healerAnimator.SetBool("Idle", true);
        PlayerActionManager.Instance.confirmPress.AddListener(HandleConfirmPress);
        InitializeActions();

    }

    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {

    }



    // void UpdatePartyUIHighlight()
    // {
    //     foreach (PartyMember partyMember in partyListContainer.GetComponentsInChildren<PartyMember>())
    //     {
    //         //Debug.Log(partyMember);
    //         //Debug.Log(partyMember.entity == targetedAlly);
    //         if (partyMember != null)
    //         {
    //             partyMember.SetHighlight(partyMember.entity == targetedAlly);
    //         }
    //     }
    // }

    void Heal(Ally target)
    {
        if (target.health < target.maxHealth)
        {
            // NOTE: Swapping targets just before the spell resolves heals the new target
            // Can be considered minor bug or a feature (When player level is high enough for mid-cast target swap)
            target.ReceiveHeal(healAmount);
            mana -= 20;
            //target.health += healAmount;
            //Debug.Log(target.name + " healed for " + healAmount);
        }
    }

    public void playHealSound(){
        healerAudio.PlayOneShot(healerAudio.clip);
    }

    void Attack(Boss boss)
    {
        boss.TakeDamage(attackPower);
    }

    // IEnumerator CastSpell(float duration)
    // {
    //     castBar.StartCasting(duration);
    //     yield return new WaitForSeconds(duration);
    //     healerAnimator.SetTrigger("CastSuccess");
    //     castingCoroutine = null;

    //     if (targetedAlly != null) 
    //     {
    //         Heal(targetedAlly);
    //     }
    // }

    // IEnumerator CastAttackSpell(float duration)
    // {
    //     castBar.StartCasting(duration);
    //     yield return new WaitForSeconds(duration);
    //     healerAnimator.SetTrigger("CastSuccess");
    //     Attack(FindFirstObjectByType<Boss>());
    //     castingCoroutine = null;
    // }


    // public override void Move(Vector2Int direction)
    // {
    //     if (!LevelManager.Instance.IsPlayerTurn()) return; // Check if player turn
    //     requestMove.Invoke(this, direction);
    //     PerformAction();
    // }

    public void PerformAction()
    {
        // Example: Moving or using an ability
        //onPlayerAction.Invoke(); // Notify LevelManager that the player acted, ok this is too premature i need an execute button
    }

    public void HandleConfirmPress()
    {
        if (!LevelManager.Instance.IsPlayerTurn()) return;
        PerformAction();
    }

    private void InitializeActions()
    {
        // Add basic actions
        actionContainer.AddAction(ScriptableObject.CreateInstance<MoveAction>());
        actionContainer.AddAction(ScriptableObject.CreateInstance<HealAction>());
        actionContainer.AddAction(ScriptableObject.CreateInstance<PlayerAttackAction>());
        availableActions.Add(ScriptableObject.CreateInstance<MoveAction>());
        availableActions.Add(ScriptableObject.CreateInstance<HealAction>());
        //availableActions.Add(ScriptableObject.CreateInstance<AttackAction>());
    }

    public void StartTurn()
    {
        currentActionPoints = maxActionPoints;
        // Notify UI to update AP display
        UpdateActionPointsDisplay?.Invoke(currentActionPoints, maxActionPoints);
    }

    public bool HasActionPoints() => currentActionPoints > 0;

    public bool TrySpendActionPoints(int cost)
    {
        if (currentActionPoints >= cost)
        {
            currentActionPoints -= cost;
            UpdateActionPointsDisplay?.Invoke(currentActionPoints, maxActionPoints);
            return true;
        }
        return false;
    }

    public List<GameAction> GetAvailableActions()
    {
        return availableActions.Where(action => action.CanExecute(this, null)).ToList();
    }



    // TODO: Add healer taking damage also, if needed
}
