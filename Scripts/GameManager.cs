using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject bar;
    public Text scoreText;
    public Text highScoreText;
    public GameObject restart;
    public float initRotSpeed = 150;
    public float rotAcceleration = 10;
    int direction;
    float rotSpeed;
    int score;
    int highScore;
    SpriteRenderer barRender;
    BarColorEnum barColorType;

    bool barRotating = false;
    bool gameStart = false;
    bool colorMatched = false;


    enum BarColorEnum
    {
        red = 1,
        yellow  =2,
        green = 3,
        blue = 4,
    }

    public void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        scoreText.text = score.ToString();//刷新分数
        highScoreText.text = highScore.ToString();//刷新最高分
        barRender = bar.GetComponent<SpriteRenderer>();
        direction = -1;
        rotSpeed = initRotSpeed;
        barColorType = (BarColorEnum)Random.Range(2, 5);
        UpdateBarColor();
    }

    void Update () {
        if (Input.GetMouseButtonDown(0) && gameStart == false)
        {
            gameStart = true;
            StartGame();
            return;
        }
        if (barRotating)
        {
            if (Input.GetMouseButtonDown(0) && gameStart == true)
            {
                direction = -direction;//每点击一次改变旋转方向
                if (ClickJudgement())//进行点击判定，正确则返回true，失败则endgame
                {
                    colorMatched = false;//将棒子和区域颜色已匹配的状态重置
                    if (score>highScore)
                    {
                        highScore = score;
                        PlayerPrefs.SetInt("HighScore", highScore);
                    }
                    scoreText.text = score.ToString();//刷新分数
                    highScoreText.text = highScore.ToString();//刷新最高分
                    ChangeBarColorType();//每点击一次将bar的颜色type随机变化
                    UpdateBarColor();//根据bar的颜色type刷新bar的显示颜色
                }
            }
            rotSpeed += rotAcceleration * Time.deltaTime;
            bar.transform.Rotate(Vector3.forward, direction * rotSpeed * Time.deltaTime);
            AreaJudgement();//进行区域判定,若超出区域未点击，则endgame
        }
    }

    private void AreaJudgement()
    {
        if (bar.transform.eulerAngles.z >= 315 || bar.transform.eulerAngles.z <= 45)
        {
            if (barColorType == BarColorEnum.red && !colorMatched) //若棒子刚进入匹配颜色的区域，则将colorMatched状态转为真
            {
                colorMatched = true;
            }
            if (colorMatched && barColorType != BarColorEnum.red) //若棒子超出其匹配颜色的区域时，endgame
            {
                EndGame();
            }
        }
        if (bar.transform.eulerAngles.z >= 225 && bar.transform.eulerAngles.z <= 315)
        {

            if (barColorType == BarColorEnum.yellow && !colorMatched)
            {
                colorMatched = true;
            }
            if (colorMatched && barColorType != BarColorEnum.yellow)
            {
                EndGame();
            }
        }
        if (bar.transform.eulerAngles.z >= 135 && bar.transform.eulerAngles.z <= 225)
        {

            if (barColorType == BarColorEnum.green && !colorMatched)
            {
                colorMatched = true;
            }
            if (colorMatched && barColorType != BarColorEnum.green)
            {
                EndGame();
            }
        }
        if (bar.transform.eulerAngles.z >= 45 && bar.transform.eulerAngles.z <= 135)
        {

            if (barColorType == BarColorEnum.blue && !colorMatched)
            {
                colorMatched = true;
            }
            if (colorMatched && barColorType != BarColorEnum.blue)
            {
                EndGame();
            }
        }
    }

    //从当前颜色进入另一个颜色时就endgame！！

    private bool ClickJudgement()
    {
        switch (barColorType)
        {
            case BarColorEnum.red:
                if (bar.transform.eulerAngles.z >= 315 || bar.transform.eulerAngles.z <= 45)
                {
                    score += 1;
                    return true;
                }
                else
                    EndGame();
                break;
            case BarColorEnum.yellow:
                if (bar.transform.eulerAngles.z >= 225 && bar.transform.eulerAngles.z <= 315)
                {
                    score += 1;
                    return true;
                }
                else
                    EndGame();
                break;
            case BarColorEnum.green:
                if (bar.transform.eulerAngles.z >= 135 && bar.transform.eulerAngles.z <= 225)
                {
                    score += 1;
                    return true;
                }
                else
                    EndGame();
                break;
            case BarColorEnum.blue:
                if (bar.transform.eulerAngles.z >= 45 && bar.transform.eulerAngles.z <= 135)
                {
                    score += 1;
                    return true;
                }
                else
                    EndGame();
                break;
        }
        return false;
    }
    public void StartGame()
    {
        barRotating = true;
    }

    private void EndGame()
    {
        barRotating = false;
        restart.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void ChangeBarColorType()
    {
        BarColorEnum randomColorType = (BarColorEnum)Random.Range(1, 5);
        if (barColorType == randomColorType)
            ChangeBarColorType();
        else
            barColorType = randomColorType;
    }

    void UpdateBarColor()
    {
        switch (barColorType)
        {
            case BarColorEnum.red:
                barRender.color = new Color(1f, 0.078f, 0.259f, 1f);
                break;
            case BarColorEnum.yellow:
                barRender.color = new Color(0.973f, 0.906f, 0.11f, 1f);
                break;
            case BarColorEnum.green:
                barRender.color = new Color(0.267f, 0.765f, 0.455f, 1f);
                break;
            case BarColorEnum.blue:
                barRender.color = new Color(0.067f, 0.698f, 0.984f, 1f);
                break;
        }
    }
}
