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
    GameObject countDownTime;
    GameObject player;
    GameObject joyStick;
    GameObject Basket;
    GameObject Camera;
    float cameraBaseDist;
    Vector3 cameraRELtarget;
    UnityEngine.UI.Text bestRecord, ScoreText;
    public bool isDymCamer;


    // Use this for initialization
    void Start()
    {
        isDymCamer = true;
        score = 0;
        best = 0;
        gameStartTime = Time.time;
        gameTime = 30;
        bestRecord = GameObject.Find("bestRecord").GetComponent<UnityEngine.UI.Text>();
        countDownTime = GameObject.Find("countDownTime");
        joyStick = GameObject.Find("joyStick");
        player = GameObject.Find("player");
        Camera = GameObject.Find("Main Camera");
        Basket = GameObject.Find("Basket");
        ScoreText = GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>();
        cameraRELtarget = Camera.transform.position - player.transform.position;
        cameraBaseDist = Vector3.Distance(player.transform.position, Basket.transform.position);
        basefov = Camera.GetComponent<Camera>().fieldOfView;

    }
    public void dymCamer()
    {
        isDymCamer = !isDymCamer;
    }
    public void throwIn()
    {
        score++;
        if (score > best)
        {
            best = score;
            bestRecord.text = "Best record: " + (best).ToString("F1");
        }
        ScoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        countDown();        //設定倒數計時
        playerControl();    //角色操作控制器
        joyStickControl();  //角色操作控制器-虛擬搖桿
        cameraFellow();     //使攝影機更隨角色
    }

    void countDown()
    {
        float gameOverTime = gameTime - (Time.time - gameStartTime);
        countDownTime.GetComponent<UnityEngine.UI.Text>().text = (gameOverTime).ToString("F1");
        if (gameOverTime <= 0)
        {
            score = 0;
            ScoreText.text = score.ToString();
            gameStartTime = Time.time;
        }

    }
    void joyStickControl()
    {
        if (joyStick.GetComponent<joyStick>().touch)
        {
            Vector2 joyStickVec = joyStick.GetComponent<joyStick>().joyStickVec;
            if (joyStickVec.x < 0)
            {
                player.GetComponent<player>().move("left", joyStickVec.x);
            }
            else
            {
                player.GetComponent<player>().move("right", joyStickVec.x);
            }
        }
    }
    void cameraFellow()
    {
        Camera.transform.position = cameraRELtarget + player.transform.position;
        if (isDymCamer)
        {
            float dist = Vector3.Distance(player.transform.position, Basket.transform.position) - cameraBaseDist;
            if (dist > 0)
            {
                Camera.GetComponent<Camera>().fieldOfView = basefov + dist * 4;
            }
            else
            {
                Camera.GetComponent<Camera>().fieldOfView = basefov;
            }
        }

    }

    void playerControl()
    {

        if (Input.GetKey("a"))
        {
            player.GetComponent<player>().move("left", 1);
        }
        if (Input.GetKey("d"))
        {
            player.GetComponent<player>().move("right", 1); ;
        }
        if (Input.GetKeyDown("b"))
        {
            player.GetComponent<player>().pressB();
        }


        if (Input.GetKeyDown("space"))
        {
            player.GetComponent<player>().jump();
        }
        if (Input.GetKeyUp("space"))
        {
            player.GetComponent<player>().fall();
        }

    }


}