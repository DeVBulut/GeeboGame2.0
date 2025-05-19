using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public float superJumpForce = 20f;
    public GameObject DeathPanel;
    public GameObject WinPanel;
    public TMP_Text winScoreText;
    public AudioSource effectAudioSource;
    public AudioSource windowAudioSource;
    public AudioClip jumpEffect;
    public AudioClip boostJumpEffect;
    public AudioClip loseEffect;
    public AudioClip winEffect;
    public AudioClip teleportEffect;
    public AudioClip breakEffect;
    public AudioClip windowEffect;
    public Transform leftBorder;
    public Transform rightBorder;

    private Rigidbody2D rb;
    public Collider2D playerCollider;
    private float horizontalInput;
    private bool alive;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float detectionTime = 0.25f;
    public int windowScore = 0;

    private float winTimer = 120f; // 1.5 minutes
    private bool gameEnded = false;
    private bool isInWinState = false;


    void Awake()
    {
        effectAudioSource = FindFirstObjectByType<AudioManager>().gameObject.transform.GetChild(0).GetComponent<AudioSource>();
    }

    void Start()
    {
        alive = true;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!alive || gameEnded) return;

        // WIN TIMER CHECK
        winTimer -= Time.deltaTime;
        if (winTimer <= 0f)
        {
            TriggerWinByTime();
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        FlipCharacter();
        HandleCollider();
        StuckDetection();
        EdgeControl();
    }

    void FixedUpdate()
    {
        if (!alive) return;
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
        if (isInWinState) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Jump(jumpForce);
            if (collision.gameObject.name.Contains("Window"))
            {

            }
            else
            {
                effectAudioSource.clip = jumpEffect;
                effectAudioSource.Play();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("SuperJump"))
        {
            Jump(superJumpForce);
            effectAudioSource.clip = boostJumpEffect;
            effectAudioSource.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Winzone"))
        {
            Debug.Log("You won!");
            TriggerWinByTime(); // Optional: treat WinZone like timeout win
        }
    }

    public void BreakSoundPlay()
    {
        if (isInWinState) return;

        effectAudioSource.clip = breakEffect;
        effectAudioSource.Play();
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
        if (rb.linearVelocity.y > 0.2)
            playerCollider.isTrigger = true;
        else
            playerCollider.isTrigger = false;
    }

    void PlayDeathSound()
    {
        effectAudioSource.clip = loseEffect;
        effectAudioSource.Play();
    }

    void PlayWinSound()
    {
        isInWinState = true;
        effectAudioSource.clip = winEffect;
        effectAudioSource.Play();
    }

    public void KillPlayer(bool hasWon)
    {
        alive = false;

        if (!hasWon)
        {
            DeathPanel.SetActive(true);
            PlayDeathSound();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(RestartAfterDelay());
        }
        // Win condition no longer needs to kill player
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private void StuckDetection()
    {
        if (Vector3.Distance(transform.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= detectionTime)
            {
                OnStuck();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    private void OnStuck()
    {
        if (isInWinState) return;

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
        if (isInWinState) return;

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

    private void TriggerWinByTime()
    {
        Debug.Log("Time's up — you win!");
        gameEnded = true;
        isInWinState = true;

        PlayWinSound();
        WinPanel.SetActive(true);
        Time.timeScale = 0f;

        if (winScoreText != null)
        {
            winScoreText.text = windowScore.ToString();
        }
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void PlayWindowSound(int code)
    {
        if (code == 1)
        {
            effectAudioSource.clip = jumpEffect;
            effectAudioSource.Play();
            return;
        }
        windowAudioSource.clip = windowEffect;
        windowAudioSource.Play();
    }
}
