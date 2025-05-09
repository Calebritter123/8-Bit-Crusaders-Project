using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystems health = collision.gameObject.GetComponent<HealthSystems>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
