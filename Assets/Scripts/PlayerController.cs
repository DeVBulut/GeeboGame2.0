using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;

    private enum CharacterState { Idle, Running, Ascending, Descending, Peak }
    private CharacterState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        FlipCharacter();
    }

    void FixedUpdate()
    {
        Move();
        CheckGrounded();
        UpdateState();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FlipCharacter()
    {
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void UpdateState()
    {
        if (isGrounded)
        {
            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                SetState(CharacterState.Running);
            }
            else
            {
                SetState(CharacterState.Idle);
            }
        }
        else
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                SetState(CharacterState.Ascending);
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                SetState(CharacterState.Descending);
            }
            else
            {
                SetState(CharacterState.Peak);
            }
        }

        HandlePlatformCollisions(); // Call the new collision handler
    }

    private void HandlePlatformCollisions()
    {
        // Enable or disable collisions based on ascending/descending state
        bool ignoreCollision = (currentState == CharacterState.Ascending);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if(ignoreCollision)
        {
            boxCollider.enabled = false; 
        }
        else
        {
            boxCollider.enabled = true;
        }

    }

    private void SetState(CharacterState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            PlayAnimationForState(newState);
        }
    }

    private void PlayAnimationForState(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.Idle:
                animator.Play("Idle");
                break;
            case CharacterState.Running:
                animator.Play("Running");
                break;
            case CharacterState.Ascending:
                animator.Play("Ascend");
                break;
            case CharacterState.Descending:
                animator.Play("Falling");
                break;
            case CharacterState.Peak:
                animator.Play("Peak");
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
