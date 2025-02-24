using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public float superJumpForce = 20f;
    public GameObject DeathPanel;
    public GameObject WinPanel;
    public AudioSource effectAudioSource;
    public AudioClip jumpEffect; 
    public AudioClip boostJumpEffect;
    public AudioClip loseEffect;
    public AudioClip winEffect;
    public AudioClip teleportEffect;
    public Transform leftBorder; 
    public Transform rightBorder;

    private Rigidbody2D rb;
    public Collider2D playerCollider;
    private float horizontalInput;
    private bool alive;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float detectionTime = 2f;
    

    void Awake()
    {

        effectAudioSource =  FindFirstObjectByType<AudioManager>().gameObject.transform.GetChild(0).GetComponent<AudioSource>();
    }

    void Start()
    {
        alive = true;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if(!alive){return;}
        horizontalInput = Input.GetAxis("Horizontal");
        FlipCharacter();
        HandleCollider();
        StuckDetection();
        EdgeControl();
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

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         Jump(jumpForce);
    //         Debug.Log(collision.gameObject.name);
    //     }
    //     else if (collision.gameObject.layer == LayerMask.NameToLayer("SuperJump"))
    //     {
    //         Jump(superJumpForce);
    //     }
    // }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Jump(jumpForce);
            effectAudioSource.clip = jumpEffect;
            effectAudioSource.Play();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("SuperJump"))
        {
            Jump(superJumpForce);
            effectAudioSource.clip = boostJumpEffect;
            effectAudioSource.Play();
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
        if (rb.linearVelocity.y > 0.2) // Ascending
        {
            playerCollider.isTrigger = true;
        }
        else if(rb.linearVelocity.y < -0.2)
        {
            playerCollider.isTrigger = false;
        }
        else
        {
            playerCollider.isTrigger = false;
        }
    }

    void PlayDeathSound()
    {
        effectAudioSource.clip = loseEffect; 
        effectAudioSource.Play();
        Debug.Log("Played Death Sound");
    }

    void PlayWinSound()
    {
        effectAudioSource.clip = winEffect; 
        effectAudioSource.Play();
        Debug.Log("Played Win Sound");
    }

    public void KillPlayer(bool hasWon)
    {
        alive = false;
        if(!hasWon)
        {
            DeathPanel.SetActive(true);
            PlayDeathSound();
        }
        else
        {
            WinPanel.SetActive(true);
            PlayWinSound();
        }
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(StartDeath());
    }

    private IEnumerator StartDeath()
    {   
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }


    private void StuckDetection()
    {
        if (Vector3.Distance(transform.position, lastPosition) < 0.01f) // Very small movement threshold
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= detectionTime)
            {
                OnStuck();
                stuckTimer = 0f; // Reset timer after triggering
            }
        }
        else
        {
            stuckTimer = 0f; // Reset timer if movement is detected
        }

        lastPosition = transform.position;
    }

    private void OnStuck()
    {
        Debug.Log("PLAYER GOT STUCK -- PUSHING THROUGH");
        playerCollider.isTrigger = true; 
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        StartCoroutine(stuckCooldown());
    }

    IEnumerator stuckCooldown()
    {
        yield return new WaitForSeconds(0.1f); 
        playerCollider.isTrigger = false; 
    }

    private void EdgeControl()
    {
        if (transform.position.x > rightBorder.position.x)
        {
            transform.position = new Vector3(leftBorder.position.x + 0.5f, transform.position.y, transform.position.z);
            effectAudioSource.clip = teleportEffect;
            effectAudioSource.Play();
        }

        if (transform.position.x < leftBorder.position.x)
        {
            transform.position = new Vector3(rightBorder.position.x - 0.5f, transform.position.y, transform.position.z);
            effectAudioSource.clip = teleportEffect;
            effectAudioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Winzone"))
        {
            Debug.Log("You won!");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            KillPlayer(true);
        }
    }

}
