using Unity.VisualScripting;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float superJumpForce = 20f;

    private Rigidbody2D rb;
    public Collider2D playerCollider;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        FlipCharacter();
        HandleCollider();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump(float jumpStrength)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrength);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Jump(jumpForce);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("SuperJump"))
        {
            Jump(superJumpForce);
        }
    }

    private void FlipCharacter()
    {
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleCollider()
    {
        if (rb.linearVelocity.y > 0) // Ascending
        {
            playerCollider.enabled = false;
        }
        else // Descending or stationary
        {
            playerCollider.enabled = true;
        }
    }

}
