using UnityEngine;

public class End : MonoBehaviour
{
    public VictoryController victoryController; // <-- Set in Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            victoryController.ShowVictory();
        }
    }
}
