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

    [Header("Platform Theme Sprites Set A")]
    public Sprite boostPlatformSprite;
    public Sprite movingPlatformSprite;
    public Sprite cloudPlatformSprite;

    [Header("Platform Theme Sprites Set B")]
    public Sprite boostPlatformSprite_B;
    public Sprite movingPlatformSprite_B;
    public Sprite cloudPlatformSprite_B;

    [Header("Platform Theme Sprites Set C")]
    public Sprite boostPlatformSprite_C;
    public Sprite movingPlatformSprite_C;
    public Sprite cloudPlatformSprite_C;

    private int themeStage = 0;

    void Start()
    {
        GeneratePlatforms();
    }

    void Update()
    {
        if(platformQueue.Count == 0) return;

        GameObject lowestPlatform = platformQueue.Peek();
        if (player.position.y > lowestPlatform.transform.position.y + recycleThreshold)
        {
            RecyclePlatform();
        }
    }

    void RecyclePlatform()
    {
        GameObject platform = platformQueue.Dequeue();
        platform.transform.position = SetPosition();

        if (platform.name.Contains("Window"))
        {
            WindowManager wm = platform.GetComponent<WindowManager>();
            if (wm != null) wm.ResetWindow();
        }

        platformQueue.Enqueue(platform);
        Debug.Log("Action performed pooling for: " + platform.name);
    }

    public void GeneratePlatforms()
    {
        if(platformPrefabs.Length == 0)
        {
            Debug.LogWarning("Platform prefab count on prefab array = 0"); 
            return;
        }
        
        for (int i = 0; i < platformPerCycle; i++)
        {
            GameObject randomPlatform = Instantiate(RandomlyGeneratedPlatform(), SetPosition(), Quaternion.identity);
            platformQueue.Enqueue(randomPlatform);
        }
    }

    private GameObject RandomlyGeneratedPlatform()
    {
        int dice = Random.Range(1, 100);
        int[] thresholds = { 35, 45, 60, 70, 80, 90, 100 };

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (dice < thresholds[i])
            {
                speicalPlatformOffset = (dice > 70 || lastPlatform.name.Contains("Window")) ? setSpecialPlatformOffset : 0f;
                lastPlatform = platformPrefabs[i];
                return platformPrefabs[i];
            }
        }

        Debug.LogWarning("Unintended Behaviour Blocker - " + platformPrefabs[0].name + " is returned");
        return platformPrefabs[0];
    }

    private Vector3 SetPosition()
    {
        float xPosition = Random.Range(-borderTransform.position.x + borderOffset, borderTransform.position.x - borderOffset);
        float yPosition = Random.Range(ySpawn + minY_DistanceBetweenPlatform, ySpawn + maxY_DistanceBetweenPlatform) + setSpecialPlatformOffset;
        ySpawn = yPosition;
        return new Vector3(xPosition, yPosition, 0);
    }

    // 🔁 Call this during background swap
    public void ApplyNextPlatformTheme()
    {
        ApplyPlatformVisualTheme(themeStage);
        themeStage++;
    }

    // 🎨 Applies a theme based on stage index
    public void ApplyPlatformVisualTheme(int stage)
    {
        foreach (GameObject platform in platformQueue)
        {
            if (platform == null) continue;

            SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            switch (platform.tag)
            {
                case "Boost":
                    sr.sprite = GetBoostSprite(stage);
                    break;
                case "Moving":
                    sr.sprite = GetMovingSprite(stage);
                    break;
                case "Cloud":
                    sr.sprite = GetCloudSprite(stage);
                    break;
            }
        }

        Debug.Log($"Platform visuals updated to theme stage {stage}");
    }

    // 🔄 Sprite pickers
    private Sprite GetBoostSprite(int stage)
    {
        switch (stage)
        {
            case 0: return boostPlatformSprite;
            case 1: return boostPlatformSprite_B;
            case 2: return boostPlatformSprite_C;
            default: return boostPlatformSprite_C;
        }
    }

    private Sprite GetMovingSprite(int stage)
    {
        switch (stage)
        {
            case 0: return movingPlatformSprite;
            case 1: return movingPlatformSprite_B;
            case 2: return movingPlatformSprite_C;
            default: return movingPlatformSprite_C;
        }
    }

    private Sprite GetCloudSprite(int stage)
    {
        switch (stage)
        {
            case 0: return cloudPlatformSprite;
            case 1: return cloudPlatformSprite_B;
            case 2: return cloudPlatformSprite_C;
            default: return cloudPlatformSprite_C;
        }
    }
}
