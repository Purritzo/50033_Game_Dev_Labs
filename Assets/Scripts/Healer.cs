using UnityEngine;

public class Healer : MonoBehaviour
{

    public int healAmount = 20;
    public Ally targetedAlly;
    public Ally[] allies;
    private bool currentlyTargetingSelf = false;
    public Vector3 startPosition;
    public TargetingManager targetingManager;

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
            targetedAlly = null;
            currentlyTargetingSelf = true;
        }
        if (Input.GetKeyDown("w"))
        {
            targetedAlly = allies[0];
            targetingManager.MoveIndicatorToAlly(targetedAlly);
            currentlyTargetingSelf = false;
        }
        if (Input.GetKeyDown("e"))
        {
            targetedAlly = allies[1];
            targetingManager.MoveIndicatorToAlly(targetedAlly);
            currentlyTargetingSelf = false;
        }
        if (Input.GetKeyDown("f"))
        {
            if (targetedAlly != null)
            {
                Heal(targetedAlly);
            }
        }
        if (Input.GetKeyDown("r"))
        {
            targetedAlly = null;
            targetingManager.MoveIndicatorToBoss(FindFirstObjectByType<Boss>());
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
