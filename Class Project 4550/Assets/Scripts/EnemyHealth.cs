using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;

    [Header("Knockback")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.15f;

    private EnemyPatrol patrolScript;
    private MeleeEnemy meleeScript;

    private void Start()
    {
        currentHealth = maxHealth;

        // ✅ Get Rigidbody2D on self or child
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = GetComponentInChildren<Rigidbody2D>();

        // ✅ Get movement scripts on self or parent
        patrolScript = GetComponent<EnemyPatrol>();
        if (patrolScript == null)
            patrolScript = GetComponentInParent<EnemyPatrol>();

        meleeScript = GetComponent<MeleeEnemy>();
        if (meleeScript == null)
            meleeScript = GetComponentInParent<MeleeEnemy>();
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        Debug.Log("TakeDamage called, direction = " + hitDirection);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ApplyKnockback(hitDirection));
        }
    }

    private System.Collections.IEnumerator ApplyKnockback(Vector2 direction)
    {
        Debug.Log("Applying knockback force: " + direction.normalized * knockbackForce);

        if (patrolScript != null) patrolScript.enabled = false;
        if (meleeScript != null) meleeScript.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
            // OR force it directly:
            // rb.velocity = direction.normalized * knockbackForce;
        }

        yield return new WaitForSeconds(knockbackDuration);

        if (patrolScript != null) patrolScript.enabled = true;
        if (meleeScript != null) meleeScript.enabled = true;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}


