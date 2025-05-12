using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    public float pointA = -5f;
    public float pointB = 5f;
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    private float moveSpeed;
    private float targetX;

    void OnEnable() // Called when object is activated in pooling
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        targetX = pointB;
    }

    void Update()
    {
        Vector3 current = transform.position;
        Vector3 target = new Vector3(targetX, current.y, current.z);

        transform.position = Vector3.MoveTowards(current, target, moveSpeed * Time.deltaTime);

        if (Mathf.Abs(current.x - targetX) < 0.1f)
        {
            targetX = (targetX == pointA) ? pointB : pointA;
        }
    }
}
