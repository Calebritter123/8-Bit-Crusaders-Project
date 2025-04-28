using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;

    public int walkingSFXIndex = 3;
    public int jumpingSFXIndex = 2;

    private VolumeControler volumeControler;

    float horizontalInput;
    public float moveSpeed = 5.1f;
    bool isFacingRight = false;
    float jumpPower = 9f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        volumeControler = FindObjectOfType<VolumeControler>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();


        if (horizontalInput != 0f && isGrounded)
        {
            if(!volumeControler.sfxSources[walkingSFXIndex].isPlaying)
            {
                volumeControler.sfxSources[walkingSFXIndex].Play();
            }
        } else if (horizontalInput == 0f && isGrounded)
        {
            if (volumeControler.sfxSources[walkingSFXIndex].isPlaying)
            {
                volumeControler.sfxSources[walkingSFXIndex].Stop();
            }
        }

        if(!isGrounded && volumeControler.sfxSources[walkingSFXIndex].isPlaying)
        {
            volumeControler.sfxSources[walkingSFXIndex].Stop();
        }


        if(Input.GetButtonDown("Jump") && isGrounded)
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
        if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }
}