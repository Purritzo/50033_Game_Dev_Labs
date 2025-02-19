using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameAction : ScriptableObject
{
    [SerializeField] protected string actionName;
    [SerializeField] protected string description;
    [SerializeField] protected int manaCost;
    [SerializeField] protected float animationDuration = 0.5f;

    // Animation trigger names - can be overridden by derived classes
    protected virtual string StartTriggerName => actionName + "Start";
    protected virtual string ExecuteTriggerName => actionName;
    protected virtual string EndTriggerName => actionName + "End";
    
    public abstract bool CanExecute(Entity executor, Entity target);
    public abstract void Execute(Entity executor, Entity target);
    public abstract bool RequiresTarget();
    public abstract Vector2Int[] GetValidTargetPositions(Entity executor);

    public string GetName() => actionName;
    public string GetDescription() => description;
    public int GetManaCost() => manaCost;

    
    // Helper method to execute action with animation
    public IEnumerator ExecuteWithAnimation(Entity executor, Entity target)
    {
        // Try to play start animation if it exists
        executor.TryPlayAnimation(StartTriggerName);
        
        // Play the main action animation
        yield return executor.PlayActionAnimation(ExecuteTriggerName);
        
        // Execute the actual action
        Execute(executor, target);
        
        // Play action effects (sound, particles, etc.)
        executor.PlayActionEffects(actionName);
        
        // Try to play end animation if it exists
        executor.TryPlayAnimation(EndTriggerName);
    }
}