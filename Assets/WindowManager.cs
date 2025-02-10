using Unity.VisualScripting;
using UnityEngine;

public class WindowManager : MonoBehaviour
{

    [SerializeField] private Sprite newSprite;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem collisionEffect; 
    public bool firstCollusion = true; 
    private void Start() 
    {
        firstCollusion = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        collisionEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && firstCollusion)
        {
            firstCollusion = false; 

            if (newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }

            if(collisionEffect != null)
            {
                collisionEffect.Play();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && firstCollusion)
        {
            firstCollusion = false; 

            if (newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }

            if(collisionEffect != null)
            {
                collisionEffect.Play();
            }
        }
    }
}
