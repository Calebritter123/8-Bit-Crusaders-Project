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

    void Start()
    {
        volumeControler = FindObjectOfType<VolumeControler>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {

        if (SceneManager.GetActiveScene().buildIndex != 1 || Menu.IsGamePaused)
        {
            if (volumeControler.sfxSources[walkingSFXIndex].isPlaying)
            {
                volumeControler.sfxSources[walkingSFXIndex].Stop();
            }
            return;
        }

        if (Menu.IsGamePaused)
        return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        // Attack trigger on left mouse click
        if (Input.GetMouseButtonDown(0) && isGrounded) // 0 = left mouse button
        {
            animator.SetTrigger("Attack");
            volumeControler.PlaySFX(attackSFXIndex);
        }

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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
