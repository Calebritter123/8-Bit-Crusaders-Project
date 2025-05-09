using UnityEngine;

public class AttackHitboxActivator : MonoBehaviour
{
    public Collider2D hitbox;

    public void EnableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.enabled = true;
            Debug.Log("Hitbox ENABLED");
        }
    }

    public void DisableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.enabled = false;
            Debug.Log("Hitbox DISABLED");
        }
    }
}
