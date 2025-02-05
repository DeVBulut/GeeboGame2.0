using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject WinScreen; 

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Winzone"))
        {
            Debug.Log("Collided");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            WinScreen.SetActive(true);
        }
    }
}
