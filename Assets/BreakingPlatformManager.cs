using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatformManager : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb;
    private void Start() 
    {
        animator = GetComponent<Animator>();   
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rb.linearVelocity.y < 0.1) // Ascending
            {
                Debug.Log(other.name + " collided");
                animator.Play("CloudBreak");    
                StartCoroutine(WaitForDestroy(5));
            }
            else // Descending or stationary
            {
                return;
            }
        }
    }

    private IEnumerator WaitForDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }
}
