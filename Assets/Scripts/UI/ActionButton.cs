using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour // This is not a good name but im not using Action keyword anyway
{

    public string actionName;
    public int potency;
    public float cooldown;
    private float lastUsedTime = -Mathf.Infinity;
    public TMPro.TextMeshProUGUI buttonText;
    public UnityEvent onActionExecute;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (buttonText != null)
        {
            buttonText.text = actionName;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Setup()
    {
        
    }
}