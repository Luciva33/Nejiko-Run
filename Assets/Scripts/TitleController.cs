using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;

    public void Start()
    {
        //ハイスコア表示
        highscoreText.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m";
    }


    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }
}
