using UnityEngine;

public class LockToPlayer : MonoBehaviour
{
    private float startXPosition;    
    public Transform playerTransform;
    private float highestYPosition; // Track the highest Y position the player has reached
    public float maxFallDistance = 5.0f; // Distance threshold before the player dies
    private bool alive = true; 

    void Start()
    {
        alive = true; 
        startXPosition = transform.position.x;
        highestYPosition = transform.position.y; // Start at the initial position
    }

    void Update()
    {
        // Only update if the player moves higher
        if (playerTransform.position.y > highestYPosition)
        {
            highestYPosition = playerTransform.position.y; // Update highest recorded Y position
        }

        // Move object only up, never down
        transform.position = new Vector3(startXPosition, highestYPosition, playerTransform.position.z);

        // Check if the player falls too far below
        if (playerTransform.position.y < highestYPosition - maxFallDistance)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        if(!alive){return;}
        if(alive){alive = false;}

        Debug.Log("Player has fallen too far! Game Over.");
        // Implement your game over logic here (disable movement, show UI, reload scene, etc.)
    }
}
