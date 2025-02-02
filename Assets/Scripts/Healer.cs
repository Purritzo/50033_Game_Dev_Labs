using UnityEngine;

public class Healer : MonoBehaviour
{

    public int healAmount = 20;
    public Ally targetedAlly;
    public Ally[] allies;
    private bool currentlyTargetingSelf = false;
    public Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            currentlyTargetingSelf = true;
        }
        if (Input.GetKeyDown("w"))
        {
            targetedAlly = allies[0];
            currentlyTargetingSelf = false;
        }
        if (Input.GetKeyDown("e"))
        {
            targetedAlly = allies[1];
            currentlyTargetingSelf = false;
        }
        if (Input.GetKeyDown("f"))
        {
            Heal(targetedAlly); // May need to add a check if an ally is targeted first or not
        }
    }

    void Heal(Ally target)
    {
        if (target.health < target.maxHealth)
        {
            target.ReceiveHeal(healAmount);
            //target.health += healAmount;
            //Debug.Log(target.name + " healed for " + healAmount);
        }
    }

    // TODO: Add healer taking damage also, if needed
}
