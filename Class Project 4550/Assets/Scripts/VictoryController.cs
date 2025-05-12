using UnityEngine;

public class VictoryController : MonoBehaviour
{
    public GameObject victoryCanvas;

    public void ShowVictory()
    {
        victoryCanvas.SetActive(true);
        Time.timeScale = 0f; // Optional: Pause the game
    }
}
