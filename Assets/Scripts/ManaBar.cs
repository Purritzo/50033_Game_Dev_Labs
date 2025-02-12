using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    public Slider slider;
    public Image fill;
    public Transform target; // The entity this HP bar follows
    public Vector3 offset; // Offset above the entity
    public Healer entity; // Reference to the entity for updating

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // very jank just whatever for now
        entity = FindFirstObjectByType<Healer>();
        setMaxHealth(entity.mana);
    }

    // Update is called once per frame
    void Update()
    {
        // Not in use, but could be useful when spawning more entities
        if (target != null)
        {
            transform.position = target.position + offset;
        }
        SetHealth(entity.mana);
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
