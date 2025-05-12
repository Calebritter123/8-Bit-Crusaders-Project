using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystems : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private HealthBar healthBar;

    private bool iframeActive;
    private float iframeDuration = 0.5f;

    private PlayerRespawn respawnSystem;
    private GameOver gameOver;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    private Rigidbody2D rb;
    private PlayerMovement movementScript;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<PlayerMovement>();
        respawnSystem = GetComponent<PlayerRespawn>();
        gameOver = FindObjectOfType<GameOver>();
    }

    public void TakeDamage(int damage)
    {
        if (iframeActive) return;

        // ✅ Always apply knockback, even when blocking
        StartCoroutine(ApplyKnockback());

        // ❌ Blocked: skip damage
        if (movementScript != null && movementScript.isBlocking)
        {
            Debug.Log("Attack was blocked!");
            return;
        }

        // ✅ Take damage only if not blocking
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(iFrame());
        }
    }

    private void Die()
    {
        if (gameOver != null)
        {
            gameOver.ShowGameOver();
            Debug.Log("Calling ShowGameOver!");
        }
        else
        {
            Debug.LogError("GameOver script not found in scene.");
        }
    }

    IEnumerator iFrame()
    {
        iframeActive = true;
        yield return new WaitForSeconds(iframeDuration);
        iframeActive = false;
    }

    IEnumerator ApplyKnockback()
    {
        if (rb != null)
        {
            float direction = movementScript != null && movementScript.transform.localScale.x < 0 ? 1f : -1f;

            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direction * knockbackForce, knockbackForce / 2), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(knockbackDuration);
    }

    void Respawn()
    {
        if (respawnSystem != null)
        {
            transform.position = PlayerRespawn.lastCheckpoint;
            currentHealth = maxHealth;

            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }
        }
        else
        {
            Debug.LogError("PlayerRespawn component not found on player.");
        }
    }
}
