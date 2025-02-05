using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

public class PartyList : MonoBehaviour
{

    public GameObject partyHPBarPreFab;
    private List<Entity> partyMembers = new List<Entity>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        partyMembers.Add(FindFirstObjectByType<Healer>());
        Ally[] sortedAllies = FindObjectsByType<Ally>(FindObjectsSortMode.None)
                      .OrderBy(ally => ally.gameObject.name) // Sort by name
                      .ToArray(); // Convert to array if needed
        foreach (Ally ally in sortedAllies){
            partyMembers.Add(ally);
        }
        InitialisePartyList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialisePartyList()
    {
        int number = 0;
        foreach (Entity member in partyMembers)
        {
            GameObject partyEntry = Instantiate(partyHPBarPreFab, new Vector3
            (
                gameObject.transform.position.x - 700, gameObject.transform.position.y + 210 - (number * 50), 0), gameObject.transform.rotation, gameObject.transform
            );
            partyEntry.name = number.ToString();
            PartyMember partyMember = partyEntry.GetComponent<PartyMember>();
            partyMember.Initialise(member);
            number += 1;
        }
    }
}
