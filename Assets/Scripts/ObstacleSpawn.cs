using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{

    public GameObject obstacle;
    public float spawnInterval = 2;
    private float timer = 0;
    public float lengthOffset = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If want instant spawn
        //SpawnObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnInterval)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    void SpawnObstacle()
    {
        float leftMostPoint = transform.position.x - lengthOffset;
        float rightMostPoint = transform.position.x + lengthOffset;
        Instantiate(obstacle, new Vector3(Random.Range(leftMostPoint, rightMostPoint), transform.position.y, 0), transform.rotation);
    }
}
