using UnityEngine;
using UnityEngine.UI;

public class CastBar : MonoBehaviour
{
    public Slider castBar;
    public Gradient gradient;
    public Image fill;
    private float castTime;
    private float castDuration;
    private bool isCasting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        castBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCasting)
        {
            castTime += Time.deltaTime;
            castBar.value = castTime / castDuration;

            if (castTime >= castDuration)
            {
                FinishCast();
            }
        }
    }

    public void StartCasting(float duration)
    {
        if (isCasting) CancelCast(); // Interrupt if already casting
        castTime = 0;
        castDuration = duration;
        castBar.value = 0;
        isCasting = true;
        castBar.gameObject.SetActive(true); // Show cast bar
    }

    public void CancelCast()
    {
        isCasting = false;
        castBar.value = 0;
        castBar.gameObject.SetActive(false); // Hide cast bar
        Debug.Log("Cast Interrupted!");
    }

    private void FinishCast()
    {
        isCasting = false;
        castBar.value = 1;
        castBar.gameObject.SetActive(false);
        Debug.Log("Spell Cast Complete!");
    }
}
