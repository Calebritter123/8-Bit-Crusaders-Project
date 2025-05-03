using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int walkingSFXIndex = 3;
    public int jumpingSFXIndex = 2;

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

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (volumeControler == null)
            volumeControler = FindObjectOfType<VolumeControler>();
    }

    void Start()
    {
        if (groundCheck == null) Debug.LogError("groundCheck is null");
        if (animator == null) Debug.LogError("animator is null");
        if (volumeControler == null) Debug.LogError("volumeControler is null");
        if (rb == null) Debug.LogError("Rigidbody2D (rb) is null");

        volumeControler = FindObjectOfType<VolumeControler>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // NEW ground detection system
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        if (horizontalInput != 0f && isGrounded)
        {
            if (!volumeControler.sfxSources[walkingSFXIndex].isPlaying)
            {
                volumeControler.sfxSources[walkingSFXIndex].Play();
            }
        }
        else if (horizontalInput == 0f && isGrounded)
        {
            if (volumeControler.sfxSources[walkingSFXIndex].isPlaying)
            {
                volumeControler.sfxSources[walkingSFXIndex].Stop();
            }
        }

        if (!isGrounded && volumeControler.sfxSources[walkingSFXIndex].isPlaying)
        {
            volumeControler.sfxSources[walkingSFXIndex].Stop();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);

            if (volumeControler.sfxSources[walkingSFXIndex].isPlaying)
            {
                volumeControler.sfxSources[walkingSFXIndex].Stop();
            }

            volumeControler.PlaySFX(jumpingSFXIndex);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
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

    // Debug circle in scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
