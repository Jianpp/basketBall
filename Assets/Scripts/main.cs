using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    int score;
    int best;
    float gameTime;
    float basefov;
    float gameStartTime;
    UnityEngine.UI.Text countDownTime;
    player player;
    joyStick joyStick;
    GameObject Basket;
    Camera Camera;
    float cameraBaseDist;
    Vector3 cameraRELtarget;
    UnityEngine.UI.Text bestRecord, ScoreText;
    public bool isDynamicCamera;


    // Use this for initialization
    void Start()
    {
        isDynamicCamera = true;
        score = 0;
        best = 0;
        gameStartTime = Time.time;
        gameTime = 30;
        bestRecord = GameObject.Find("bestRecord").GetComponent<UnityEngine.UI.Text>();
        countDownTime = GameObject.Find("countDownTime").GetComponent<UnityEngine.UI.Text>();
        joyStick = GameObject.Find("joyStick").GetComponent<joyStick>();
        player = GameObject.Find("player").GetComponent<player>();
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Basket = GameObject.Find("Basket");
        ScoreText = GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>();
        cameraRELtarget = Camera.transform.position - player.transform.position;
        cameraBaseDist = Vector3.Distance(player.transform.position, Basket.transform.position);
        basefov = Camera.GetComponent<Camera>().fieldOfView;

    }

    // Update is called once per frame
    void Update()
    {
        countDown();        //設定倒數計時
        playerControl();    //角色操作控制器
        joyStickControl();  //角色操作控制器-虛擬搖桿
        cameraFellow();     //使攝影機更隨角色
    }
    public void DynamicCamera()
    {
        isDynamicCamera = !isDynamicCamera;
    }
    public void throwIn()
    {
        addScroe();             //增加得分
        refreashBestRecord();   //更新最高得分紀錄
        UpdateScoreText();      //更新最高得分顯示

    }
    void countDown()
    {
        float gameOverTime = gameTime - (Time.time - gameStartTime);
        countDownTime.text = (gameOverTime).ToString("F1");
        if (gameOverTime <= 0)
        {
            score = 0;
            ScoreText.text = score.ToString();
            gameStartTime = Time.time;
        }

    }
    void joyStickControl()
    {
        if (joyStick.touch)
        {
            if (joyStick.joyStickVec.x < 0)
            {
                player.move("left", joyStick.joyStickVec.x);
            }
            else
            {
                player.move("right", joyStick.joyStickVec.x);
            }
        }
    }
    void cameraFellow()
    {
        Camera.transform.position = cameraRELtarget + player.transform.position;
        if (isDynamicCamera)
        {
            float dist = Vector3.Distance(player.transform.position, Basket.transform.position) - cameraBaseDist;
            if (dist > 0)
            {
                Camera.fieldOfView = basefov + dist * 4;
            }
            else
            {
                Camera.fieldOfView = basefov;
            }
        }

    }

    void playerControl()
    {

        if (Input.GetKey("a"))
        {
            player.move("left", 1);
        }
        if (Input.GetKey("d"))
        {
            player.move("right", 1); ;
        }
        if (Input.GetKeyDown("b"))
        {
            player.pressB();
        }


        if (Input.GetKeyDown("space"))
        {
            player.jump();
        }
        if (Input.GetKeyUp("space"))
        {
            player.fall();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            player.chageMode();
        }

    }
    void refreashBestRecord()
    {
        if (score > best)
        {
            best = score;
            bestRecord.text = "Best record: " + (best).ToString("F1");
        }
    }
    void addScroe()
    {
        score++;

    }
    void UpdateScoreText()
    {
        ScoreText.text = score.ToString();
    }

}