using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;
    public Transform playerTransform; // Assign in Inspector (your Player's transform)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
            if (enemy != null && playerTransform != null)
            {
                Vector2 knockbackDir = other.transform.position - playerTransform.position;
                enemy.TakeDamage(damage, knockbackDir);
                Debug.Log("Attack hit enemy: " + other.name);
            }
        }
    }
}
