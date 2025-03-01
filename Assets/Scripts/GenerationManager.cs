using Unity.VisualScripting;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    //General Script Manager for Generation Classes

    [Header("Generation Prefabs")]
    public GameObject[] platformPrefabs;
    public GameObject winZone;
    [SerializeField] public Transform borderTransform; 

    [Header("Generation Variables")]
    [SerializeField] private float minY_DistanceBetweenPlatform = 1.5f; 
    [SerializeField] private float maxY_DistanceBetweenPlatform = 3.5f;

    [Tooltip("The starting Y Vector for platform generation")]
    [SerializeField] private float currentPlatformY;
    private GameObject lastPlatform;

    public int platformPerCycle = 200; 
    public float averageSpawnInterval = 0.5f;
    public float spawnIntervalChange = 1f;




    private void Start()
    {
        //GeneratePlatforms(250, 1, 2);
        lastPlatform = Instantiate(platformPrefabs[0], Vector3.zero, Quaternion.identity); 
    }

    public void GeneratePlatforms()
    {
        //fail Check
        if(platformPrefabs.Length == 0){Debug.LogWarning("Platform prefab count on prefab array = 0"); return;}
        
        //Type selection and position loop.
        for (int i = 0; i < platformPerCycle; i++)
        {
            GameObject randomPlatform = RandomlyGeneratedPlatform();
            lastPlatform = randomPlatform;
            Vector3 spawnLocation = SetPosition(averageSpawnInterval, spawnIntervalChange);

            Instantiate(randomPlatform, spawnLocation, Quaternion.identity); 
        }
    }

    private Vector3 SetPosition(float averageSpawnInterval, float spawnIntervalChange)
    {
        float yPosition = lastPlatform.transform.position.y; 

        float xSpawnPosition = Random.Range(-borderTransform.position.x, borderTransform.position.x);
        float ySpawnPosition = Random.Range(yPosition + averageSpawnInterval,  yPosition + averageSpawnInterval + spawnIntervalChange);
        
        Vector3 spawnLocation = new Vector3(xSpawnPosition, yPosition, 0);
        return spawnLocation;
    }

    private GameObject RandomlyGeneratedPlatform()
    {
        int dice = Random.Range(1, 100);
        int[] weights = { 35, 10, 15, 10, 10, 10, 10 }; 
        
        int cumulative = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];

            if (dice < cumulative)
            {
                // Special case: Prevent repetitive "WindowPlatform" spawns
                if (dice > 88 && (lastPlatform.name.Contains("WindowPlatform_Variant")))
                {
                    return platformPrefabs[0]; // Return first(default) platform
                }
                return platformPrefabs[i];
            }
        }

        Debug.LogWarning("Unintended Behaviour Blocker - " + platformPrefabs[0].name + " is returned");
        return platformPrefabs[0]; // Fallback (should never happen)
    }    
}
