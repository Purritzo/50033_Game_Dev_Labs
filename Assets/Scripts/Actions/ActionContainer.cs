using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionContainer : MonoBehaviour
{
    [SerializeField] private List<GameAction> actions = new List<GameAction>();

    public List<GameAction> GetAvailableActions()
    {
        return actions.Where(action => action.CanExecute(GetComponent<Entity>(), null)).ToList();
    }

    public List<GameAction> GetAllActions()
    {
        return actions;
    }

    public void AddAction(GameAction action)
    {
        if (!actions.Contains(action))
        {
            actions.Add(action);
        }
    }
}