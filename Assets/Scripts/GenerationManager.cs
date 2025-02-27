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
    private float lastPlatformPosition; 

    
    public void GeneratePlatforms()
    {
        //fail Check
        if(platformPrefabs.Length == 0){Debug.LogWarning("Platform prefab count on prefab array = 0"); return;}
        
        for (int i = 0; i < platformPerCycle; i++)
        {
            GameObject randomPlatform = RandomlyGeneratedPlatform();
        }

    }

    private GameObject RandomlyGeneratedPlatform()
    {
        return null; 
    }
}
