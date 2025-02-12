using System.Collections;
using UnityEngine;

public class BasicExpandingTelegraph : MonoBehaviour
{

    public GameObject outerCircle;  // Reference to the outer warning zone
    public GameObject innerCircle;  // Expanding attack zone
    public float telegraphTime = 3f;
    public float maxSize = 5f;       // How big the attack expands
    public float innerMaxAlpha = 0.7f; // How visible the attack becomes

    private SpriteRenderer outerRenderer;
    private SpriteRenderer innerRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outerRenderer = outerCircle.GetComponent<SpriteRenderer>();
        innerRenderer = innerCircle.GetComponent<SpriteRenderer>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWarning(){
        StartCoroutine(ExpandTelegraph());
    }

    IEnumerator ExpandTelegraph()
    {
        //gameObject.SetActive(true);
        float elapsedTime = 0f;
        Vector3 originalSize = innerCircle.transform.localScale;

        // Color outerColor = outerRenderer.color;
        // Color innerColor = innerRenderer.color;

        while (elapsedTime < telegraphTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / telegraphTime; // Normalized time (0 to 1)

            // Expand the inner telegraph zone
            innerCircle.transform.localScale = Vector3.Lerp(originalSize, Vector3.one * maxSize, t);

            // Increase inner opacity
            //innerRenderer.color = new Color(innerColor.r, innerColor.g, innerColor.b, Mathf.Lerp(0f, innerMaxAlpha, t));

            yield return null;
        }

        // Attack lands here (Trigger damage or next phase)
        //Destroy(gameObject); // Remove telegraph after attack lands
        gameObject.SetActive(false);
    }
}
