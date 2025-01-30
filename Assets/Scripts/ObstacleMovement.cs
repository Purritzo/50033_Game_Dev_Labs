using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    //public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
