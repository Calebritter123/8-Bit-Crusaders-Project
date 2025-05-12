using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public int walkingSFXIndex = 3;
    public int jumpingSFXIndex = 2;
    public int attackSFXIndex = 5;

    private VolumeControler volumeControler;

    float horizontalInput;
    public float moveSpeed = 5.1f;
    bool isFacingRight = false;
    float jumpPower = 9f;
    bool isGrounded = false;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    Animator animator;

    public bool isBlocking { get; private set; } = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // ✅ Make player persist between scenes
    }

    void Start()
    {
        volumeControler = FindObjectOfType<VolumeControler>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Menu.IsGamePaused)
        {
            if (volumeControler.sfxSources[walkingSFXIndex].isPlaying)
                volumeControler.sfxSources[walkingSFXIndex].Stop();
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        // ✅ Blocking input (allowed in air or ground)
        isBlocking = Input.GetMouseButton(1);
        animator.SetBool("IsBlocking", isBlocking);

        // ✅ Attack input (only if not blocking)
        if (Input.GetMouseButtonDown(0) && !isBlocking)
        {
            animator.SetTrigger("Attack");
            volumeControler.PlaySFX(attackSFXIndex);
        }

        // ✅ Jump input (even while blocking)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);

            if (volumeControler.sfxSources[walkingSFXIndex].isPlaying)
                volumeControler.sfxSources[walkingSFXIndex].Stop();

            volumeControler.PlaySFX(jumpingSFXIndex);
        }

        // ✅ Walking SFX logic
        if (horizontalInput != 0f && isGrounded && !isBlocking)
        {
            if (!volumeControler.sfxSources[walkingSFXIndex].isPlaying)
                volumeControler.sfxSources[walkingSFXIndex].Play();
        }
        else
        {
            if (volumeControler.sfxSources[walkingSFXIndex].isPlaying)
                volumeControler.sfxSources[walkingSFXIndex].Stop();
        }
    }

    private void FixedUpdate()
    {
        // ✅ Freeze movement only while blocking on ground
        if (isBlocking && isGrounded)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
