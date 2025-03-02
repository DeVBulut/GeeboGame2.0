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
    [SerializeField] private float ySpawn = -5f; 
    [SerializeField] private int platformPerCycle;
    [SerializeField] private GameObject lastPlatform;
    [SerializeField] private float borderOffset;

    public void GeneratePlatforms()
    {
        //fail Check
        if(platformPrefabs.Length == 0){Debug.LogWarning("Platform prefab count on prefab array = 0"); return;}
        
        for (int i = 0; i < platformPerCycle; i++)
        {
            GameObject randomPlatform = RandomlyGeneratedPlatform();
            Vector3 spawnLocation = SetPosition();
            Instantiate(randomPlatform, spawnLocation, Quaternion.identity); 
        }
    }

    private GameObject RandomlyGeneratedPlatform()
    {
        int dice = Random.Range(1, 100);
        int[] thresholds = { 35, 45, 60, 70, 80, 90, 100};

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (dice < thresholds[i])
            {
                if(dice > 70 && lastPlatform.name.Contains("Window"))
                {
                    lastPlatform = platformPrefabs[0];
                    return platformPrefabs[0];
                }
                lastPlatform = platformPrefabs[i];
                return platformPrefabs[i];
            }
        }

        Debug.LogWarning("Unintended Behaviour Blocker - " + platformPrefabs[0].name + " is returned");
        return platformPrefabs[0]; // Fallback (should never happen)
    }    

    private Vector3 SetPosition()
    {
        float xPosition = Random.Range(-borderTransform.position.x + borderOffset, borderTransform.position.x - borderOffset);
        float yPosition = Random.Range(ySpawn + minY_DistanceBetweenPlatform , ySpawn + maxY_DistanceBetweenPlatform);
        ySpawn = yPosition;
        Debug.Log("Randomized Position is x: " + xPosition + " y: " + yPosition);
        return new Vector3(xPosition, yPosition, 0);
    }
}
