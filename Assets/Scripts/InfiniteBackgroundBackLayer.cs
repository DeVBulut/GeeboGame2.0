using UnityEngine;

public class InfiniteBackgroundMultiSwap : MonoBehaviour
{
    public Transform player;
    public GameObject[] backgrounds; // 3 active background GameObjects
    public GameObject[] swapSet1;    // first replacement set
    public GameObject[] swapSet2;    // second replacement set
    public float backgroundHeight;

    private int middleIndex = 1;
    private int lowestIndex = 0;

    private float timeElapsed = 0f;
    private bool swapPending = false;
    private int recycledCount = 0;
    public int swapStage = 0; // 0 = original, 1 = swapSet1 active, 2 = swapSet2 active

    [SerializeField] private GenerationManager generationManager;

    void Start()
    {
        if (backgrounds.Length != 3 || swapSet1.Length != 3 || swapSet2.Length != 3)
        {
            Debug.LogError("Each background set must contain exactly 3 objects.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (!swapPending && swapStage == 0 && timeElapsed >= 30f)
        {
            BeginSwap();
        }
        else if (!swapPending && swapStage == 1 && timeElapsed >= 60f)
        {
            BeginSwap();
        }

        if (player.position.y > backgrounds[middleIndex].transform.position.y)
        {
            RepositionBackground();
        }
    }

    void BeginSwap()
    {
        swapPending = true;
        recycledCount = 0;
    }

    void RepositionBackground()
    {
        if (backgrounds.Length == 0) return;

        int highestIndex = (lowestIndex + backgrounds.Length - 1) % backgrounds.Length;
        float newY = backgrounds[highestIndex].transform.position.y + backgroundHeight;

        backgrounds[lowestIndex].transform.position = new Vector3(
            backgrounds[lowestIndex].transform.position.x,
            newY,
            backgrounds[lowestIndex].transform.position.z
        );

        if (swapPending)
        {
            recycledCount++;

            if (recycledCount >= 3)
            {
                PerformSwap();
                swapPending = false;
                swapStage++;
            }
        }

        lowestIndex = (lowestIndex + 1) % backgrounds.Length;
        middleIndex = (middleIndex + 1) % backgrounds.Length;
    }

    void PerformSwap()
    {
        GameObject[] nextSet = null;

        if (swapStage == 0) nextSet = swapSet1;
        else if (swapStage == 1) nextSet = swapSet2;

        if (nextSet == null)
        {
            Debug.LogWarning("No further swap set available.");
            return;
        }

        Debug.Log($"Swapping to set {swapStage + 1}...");

        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = backgrounds[i].transform.position;
            Quaternion rot = backgrounds[i].transform.rotation;
            Transform parent = backgrounds[i].transform.parent;

            Destroy(backgrounds[i]);

            GameObject newBG = Instantiate(nextSet[i], pos, rot, parent);
            backgrounds[i] = newBG;
        }

        if (generationManager != null)
        {
            generationManager.ApplyNextPlatformTheme();
        }
    }
}
