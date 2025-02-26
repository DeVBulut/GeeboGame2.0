using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform player; // Assign the player
    public GameObject[] backgrounds; // Assign 5 background pieces in order
    public float backgroundHeight; // Height of a single background piece

    private int middleIndex = 2; // The third background in the array
    private int lowestIndex = 0; // The lowest background in the array

    void Update()
    {
        // Check if the player has moved past the middle background
        if (player.position.y > backgrounds[middleIndex].transform.position.y)
        {
            RepositionBackground();
        }
    }

    void RepositionBackground()
    {
        //Fail Checker
        if(backgrounds.Length == 0){Debug.LogWarning("No Background Added to Background Array"); return;}

        // Get the highest background position
        int highestIndex = (lowestIndex + backgrounds.Length - 1) % backgrounds.Length;
        float newY = backgrounds[highestIndex].transform.position.y + backgroundHeight;

        // Move the lowest background to the top
        backgrounds[lowestIndex].transform.position = new Vector3(backgrounds[lowestIndex].transform.position.x, newY, backgrounds[lowestIndex].transform.position.z);

        // Update indices to keep track of the correct order
        lowestIndex = (lowestIndex + 1) % backgrounds.Length;
        middleIndex = (middleIndex + 1) % backgrounds.Length;
    }
}
