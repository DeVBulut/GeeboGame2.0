using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    public float pointA = -5f; // First X position
    public float pointB = 5f;  // Second X position
    public float minSpeed = 2f; // Minimum speed
    public float maxSpeed = 5f; // Maximum speed

    private float moveSpeed;
    private float targetX;
    private float fixedY; // Stores the platform's Y position
    private float fixedZ; // Stores the platform's Z position

    void Start()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed); // Get a random speed
        targetX = pointB; // Start moving towards point B
        fixedY = transform.position.y; // Store initial Y position
        fixedZ = transform.position.z; // Store initial Z position
    }

    void Update()
    {
        // Move only along the X-axis while keeping Y and Z stable
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(targetX, fixedY, fixedZ), // Lock Y and Z
            moveSpeed * Time.deltaTime
        );

        // Check if the platform reached the target X position, then switch direction
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            targetX = (targetX == pointA) ? pointB : pointA;
        }
    }
}
