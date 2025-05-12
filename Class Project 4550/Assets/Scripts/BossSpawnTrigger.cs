using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSpawnTrigger : MonoBehaviour
{
    public GameObject bossToSpawn;
    public AudioClip bossMusic; // ✅ Drag your boss music clip here

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

                // ✅ Trigger the boss spawn animation
                Animator bossAnimator = bossToSpawn.GetComponent<Animator>();
                if (bossAnimator != null)
                {
                    bossAnimator.Play("Boss_Spawn", 0, 0f);
                }
            }

            // ✅ Play boss music immediately
            VolumeControler vc = FindObjectOfType<VolumeControler>();
            if (vc != null && bossMusic != null)
            {
                vc.bgmSource.clip = bossMusic;
                vc.bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
                vc.bgmSource.Play();
            }

            Destroy(gameObject); // Remove the trigger after use
        }
    }
}
