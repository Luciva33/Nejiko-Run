using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public NejikoController nejiko;
    public TextMeshProUGUI scoreText;
    public LifePanel lifePanel;


    public void Update()
    {
        //スコアの更新
        int score = CalcScore();
        scoreText.text = "Score : " + score + "m";

        //ライフパネルを更新
        lifePanel.UpdateLife(nejiko.Life());
        //ネジコのライフが０ならゲームオーバー
        if (nejiko.Life() <= 0)
        {
            //これ以降のUpdateは止める
            enabled = false;

            //ハイスコアを更新
            if (PlayerPrefs.GetInt("HighScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            //2秒後にReturnToTitleを呼び出す
            Invoke("ReturnToTitle", 3.0f);
        }

    }

    int CalcScore()
    {
        //ネジコの走行距離をスコアとする
        return (int)nejiko.transform.position.z;
    }

    void ReturnToTitle()
    {
        //タイトルに切り替え
        SceneManager.LoadScene("Title");
    }

}
