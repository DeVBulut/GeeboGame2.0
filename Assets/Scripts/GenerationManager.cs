using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float setSpecialPlatformOffset;
    private float speicalPlatformOffset; 
    public Queue<GameObject> platformQueue = new Queue<GameObject>();
    [SerializeField] private float recycleThreshold;
    public Transform player;


    void Update()
    {
        if(platformQueue.Count == 0){return;}
        GameObject lowestPlatform = platformQueue.Peek();
        if (player.position.y > lowestPlatform.transform.position.y + recycleThreshold)
        {
            RecyclePlatform();
        }
    }

    void RecyclePlatform()
    {
        // Get the lowest platform and reposition it to the front
        GameObject platform = platformQueue.Dequeue();
        platform.transform.position = SetPosition();
        
        // Add it back to the pool
        platformQueue.Enqueue(platform);

        Debug.Log("Action performed pooling for : " + platform.name);
    }

    public void GeneratePlatforms()
    {
        //fail Check
        if(platformPrefabs.Length == 0){Debug.LogWarning("Platform prefab count on prefab array = 0"); return;}
        
        for (int i = 0; i < platformPerCycle; i++)
        {
            GameObject randomPlatform = Instantiate(RandomlyGeneratedPlatform(), SetPosition(), Quaternion.identity);
            platformQueue.Enqueue(randomPlatform);
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
                if(dice > 70 || lastPlatform.name.Contains("Window"))
                {
                    speicalPlatformOffset = setSpecialPlatformOffset;
                }
                else{speicalPlatformOffset = 0f;}
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
        yPosition += setSpecialPlatformOffset;

        ySpawn = yPosition;
        //Debug.Log("Randomized Position is x: " + xPosition + " y: " + yPosition);
        return new Vector3(xPosition, yPosition, 0);
    }


}
