using Unity.VisualScripting;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    //General Script Manager for Generation Classes

    [Header("Generation Prefabs")]
    public GameObject[] platformPrefabs;
    public GameObject winZone;
    [SerializeField] private Transform borderTransform; 

    [Header("Generation Variables")]
    public int platformPerCycle;
    [SerializeField] private float minY_DistanceBetweenPlatform = 1.5f; 
    [SerializeField] private float maxY_DistanceBetweenPlatform = 3.5f;

    [Tooltip("The starting Y Vector for platform generation")]
    [SerializeField] private float startY = 3.5f;
    private GameObject lastPlatform; 

    
    public void GeneratePlatforms()
    {
        //fail Check
        if(platformPrefabs.Length == 0){Debug.LogWarning("Platform prefab count on prefab array = 0"); return;}
        
        for (int i = 0; i < platformPerCycle; i++)
        {
            GameObject randomPlatform = RandomlyGeneratedPlatform();
            lastPlatform = randomPlatform;
        }

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
