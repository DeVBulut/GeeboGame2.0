using UnityEngine;

public class LockToPlayer : MonoBehaviour
{
    private float startXPositiion;    
    public Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startXPositiion = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(startXPositiion, playerTransform.position.y, playerTransform.position.z);
    }
}
