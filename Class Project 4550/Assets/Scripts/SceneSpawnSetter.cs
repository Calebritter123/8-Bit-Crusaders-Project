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
                player.GetComponent<PlayerRespawn>().Respawn();
            }
        }
    }
}
