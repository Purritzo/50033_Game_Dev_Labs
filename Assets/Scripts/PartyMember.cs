using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI HPText;
    public Slider hpBar;
    private Entity entity; // Reference to the entity for updating

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (entity != null)
        {
            hpBar.value = entity.health; // Continuously update HP bar
            HPText.text = entity.health.ToString();
        }
    }

    public void Initialise(Entity assignedEntity)
    {
        entity = assignedEntity;
        nameText.text = entity.entityName;
        hpBar.maxValue = entity.maxHealth;
        hpBar.value = entity.health;
    }
}
