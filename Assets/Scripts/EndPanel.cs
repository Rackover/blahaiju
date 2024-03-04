using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndPanel : MonoBehaviour
{
    public string sceneName;
    public string menuSceneName;
    public TextMeshProUGUI scoreText;
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
        scoreText.text = $"Score: {Score.Instance.score}";
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);

    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneName);
    }
}
