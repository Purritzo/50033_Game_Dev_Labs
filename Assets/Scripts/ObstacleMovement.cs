using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    //public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 startPosition;
    public float moveSpeed = 5;
    public float deadZone = -10;
    public LogicScript logic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.localPosition;
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.down * moveSpeed) * Time.deltaTime;
        if (transform.position.y < deadZone)
        {
            Debug.Log("Obstacle deleted");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            logic.addScore(1);
        }
    }
}
