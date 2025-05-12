using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{
    public GameObject bossToSpawn;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (bossToSpawn != null)
            {
                bossToSpawn.SetActive(true);

                // Trigger animation
                Animator bossAnimator = bossToSpawn.GetComponent<Animator>();
                if (bossAnimator != null)
                {
                    bossAnimator.SetTrigger("spawn");
                }
            }

            Destroy(gameObject);
        }
    }
}
