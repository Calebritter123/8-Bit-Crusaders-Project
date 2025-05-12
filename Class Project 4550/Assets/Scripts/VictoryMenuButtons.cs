using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenuButtons : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuPrototype"); 
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); // This logs in editor
        Application.Quit();     // This works in a built version
    }
}
