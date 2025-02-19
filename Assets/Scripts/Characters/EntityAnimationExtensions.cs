using UnityEngine;
using System.Collections;

public static class EntityAnimationExtensions
{
    // Duration to wait if no animation is found
    private const float DEFAULT_ACTION_DURATION = 0.5f;

    public static bool TryPlayAnimation(this Entity entity, string triggerName)
    {
        if (entity.TryGetComponent<Animator>(out var animator) && 
            HasParameter(animator, triggerName))
        {
            animator.SetTrigger(triggerName);
            return true;
        }
        return false;
    }

    public static void SetAnimationState(this Entity entity, string boolName, bool state)
    {
        if (entity.TryGetComponent<Animator>(out var animator) && 
            HasParameter(animator, boolName))
        {
            animator.SetBool(boolName, state);
        }
    }

    private static bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    public static IEnumerator PlayActionAnimation(this Entity entity, string actionName)
    {
        bool hasAnimation = entity.TryPlayAnimation(actionName);
        
        if (hasAnimation)
        {
            // If we have an animation, wait for its duration
            if (entity.TryGetComponent<Animator>(out var animator))
            {
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForSeconds(state.length);
            }
        }
        else
        {
            // If no animation exists, wait for default duration
            yield return new WaitForSeconds(DEFAULT_ACTION_DURATION);
        }
    }

    // Helper method to play common effects
    public static void PlayActionEffects(this Entity entity, string actionName)
    {
        // Play sound if available
        if (entity.TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource.Play();
        }

        // Could add particle effects, screen shake, etc. here
    }
}