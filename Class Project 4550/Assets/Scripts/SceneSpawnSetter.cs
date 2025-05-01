using UnityEngine;

public class SceneSpawnSetter : MonoBehaviour
{
    void Start()
    {
        GameObject spawnPoint = GameObject.Find("PlayerSpawn");

        if (spawnPoint != null)
        {
            PlayerRespawn.lastCheckpoint = spawnPoint.transform.position;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Move player to spawn point
                player.GetComponent<PlayerRespawn>().Respawn();

                // FIX: Nudge player down and wake physics
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero; // Reset velocity
                    rb.MovePosition(PlayerRespawn.lastCheckpoint - new Vector3(0f, 0.05f, 0f)); // Force grounding
                    rb.WakeUp(); // Reactivate physics
                }
            }
        }
    }
}
