using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelMenu : MonoBehaviour
{
    public void OpenLevel(int levelId){
        string levelName = "Scene" + levelId;
        SceneManager.LoadScene(levelName);
    }
}
