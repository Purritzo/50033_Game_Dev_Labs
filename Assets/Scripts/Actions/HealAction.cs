using System.Collections.Generic;
using UnityEngine;

public class HealAction : GameAction
{
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int healAmount = 20;

    private void OnEnable()
    {
        actionName = "Heal";
        description = "Restore HP to target";
        manaCost = 20;
    }

    public override bool CanExecute(Entity executor, Entity target)
    {
        Healer healer = executor as Healer;
        if (healer == null || !healer.HasActionPoints()) return false;
        if (healer.mana < manaCost) return false;

        return true;
        
        // Below is if checking to confirm.. which is okay with immediate usage on self
        //return target != null && target.health < target.maxHealth;
    }

    public override void Execute(Entity executor, Entity target)
    {
        Healer healer = executor as Healer;
        if (healer == null) return;

        if (healer.TrySpendActionPoints(actionPointCost))
        {
            healer.mana -= manaCost;
            target.ReceiveHeal(healAmount);
            
            // Play animation and effects
            healer.healerAnimator.SetTrigger("CastSuccess");
            healer.playHealSound();
        }
    }

    public override Vector2Int[] GetValidTargetPositions(Entity executor)
    {
        //TBC
        List<Vector2Int> validPositions = new List<Vector2Int>();
        return validPositions.ToArray();
    }

    
    public override bool RequiresTarget(){
        return true;
    }
}