using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{

    public float moveSpeed = 1.5f;
    public float lifetime = 1f;
    public Text textMesh;

    private Color textColor;
    private float elapsedTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // Fade out
        textColor.a = Mathf.Lerp(1, 0, elapsedTime / lifetime);
        textMesh.color = textColor;

        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(string text, Color color)
    {
        textMesh.text = text;
        textColor = color;
        textMesh.color = textColor;
    }
}
