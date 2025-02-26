using Unity.Collections;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public GameObject winZone; 
    public Transform spawnAreaMin; // Assign the minimum spawn position
    public Transform spawnAreaMax; // Assign the maximum spawn position

    public float minYDistance = 1.5f; // Minimum Y gap
    public float maxYDistance = 3.5f; // Maximum Y gap

    //public int platformCount = 10; // Number of platforms to spawn

    private float lastYPosition; // Keeps track of last Y position

    [SerializeField] private GameObject lastPlatform;

    void Start()
    {
        lastPlatform = platformPrefabs[0];
        // Start spawning from minY area
        //lastYPosition = spawnAreaMin.position.y;
    }

    public void SpawnPlatforms(int platformCount)
    {
        //Fail Checker
        if(platformPrefabs.Length == 0){Debug.LogWarning("Platform prefab count on prefab array = 0"); return;}


        Time.timeScale = 1; 
        float yPosition = this.transform.position.y;
        for (int i = 0; i < platformCount; i++)
        {
            GameObject selectedPlatform = SelectPlatform();
            lastPlatform = selectedPlatform;

            bool isWindow = selectedPlatform.name == "WindowPlatform_Variant_1" || selectedPlatform.name == "WindowPlatform_Variant_2" || selectedPlatform.name == "WindowPlatform_Variant_3";
            
            float randomX = 
            !isWindow ? Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x ) : Random.Range(spawnAreaMin.position.x + 0.5f, spawnAreaMax.position.x - 0.5f); 

            float randomYIncrease = !isWindow ? Random.Range(minYDistance - 0.3f , maxYDistance) : Random.Range(minYDistance + 0.5f, maxYDistance + 0.5f); 

            yPosition += randomYIncrease;

            // Create the spawn position
            Vector3 spawnPosition = new Vector3(randomX, yPosition, this.transform.position.z);

            // Spawn the platform
            Instantiate(selectedPlatform, spawnPosition, Quaternion.identity);

            if(i == platformCount - 1) {Instantiate(winZone, new Vector3(0, yPosition, this.transform.position.z), Quaternion.identity);}
        }

    }

    public GameObject SelectPlatform()
    {
        int dice = Random.Range(1, 100);

        int[] thresholds = { 35, 45, 60, 70, 80, 90, 100};

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (dice < thresholds[i])
            {
                if(dice > 88 && (lastPlatform.name == "WindowPlatform_Variant_1" || lastPlatform.name == "WindowPlatform_Variant_2" || lastPlatform.name == "WindowPlatform_Variant_3"))
                {
                    return platformPrefabs[0];
                }
                return platformPrefabs[i];
            }
        }

        throw new System.Exception("Invalid dice roll value");
    }

    //--> This Code Block skips the window Platforms from spawning if inserted into the for loop inside spawnplatforms. 

    // if(selectedPlatform.name == "WindowPlatform_Variant_1")
            // {
            //     continue;
            // }
            // else if(selectedPlatform.name == "WindowPlatform_Variant_2")
            // {
            //     continue; 
            // }
            // else if(selectedPlatform.name == "WindowPlatform_Variant_3")
            // {
            //     continue; 
            // }
}
