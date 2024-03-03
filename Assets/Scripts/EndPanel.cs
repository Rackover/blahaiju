using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{
    public string sceneName;
    public string menuSceneName;
    public GameObject winTitle;
    public GameObject loseTitle;
    public void Initialize(bool _won)
    {
        if (_won)
        {
            winTitle.SetActive(true);
            loseTitle.SetActive(false);
        }
        else
        {
            winTitle.SetActive(false);
            loseTitle.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
