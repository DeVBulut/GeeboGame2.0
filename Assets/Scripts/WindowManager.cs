using Unity.VisualScripting;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private Sprite newSprite;
    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem collisionEffect;
    public bool firstCollusion = true;

    private void Awake() // safer than Start for initialization when using pooling
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite; // store the starting sprite
        }

        collisionEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && firstCollusion)
        {
            GameObject player = collision.gameObject;
            HandleCollision(player);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && firstCollusion)
        {
            GameObject player = collision.gameObject;
            HandleCollision(player);
        }
        else if (collision.gameObject.CompareTag("Player") && !firstCollusion)
        {
            GameObject player = collision.gameObject;
            CharacterController2D controller = player.GetComponent<CharacterController2D>();
            if (controller != null)
            {
                controller.PlayWindowSound(1);
            }
        }
    }

    private void HandleCollision(GameObject player)
    {
        firstCollusion = false;

        if (newSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }

        if (collisionEffect != null)
        {
            collisionEffect.Play();
        }

        // Add to player's window score
        if (player != null)
        {
            CharacterController2D controller = player.GetComponent<CharacterController2D>();
            if (controller != null)
            {
                controller.windowScore += 1;
                controller.PlayWindowSound(0);
            }
        }
    }


    public void ResetWindow()
    {
        firstCollusion = true;

        if (spriteRenderer != null && originalSprite != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}
