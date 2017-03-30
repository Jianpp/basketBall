using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class player : MonoBehaviour
{
    public bool IsGrounded;
    public bool isTake;
    GameObject[] allTakeable;
    GameObject closetBall;
    GameObject newBall;
    Transform lastclosetBall;
    Vector3 lastclosetBallScale;

    UnityEngine.UI.Text velocityText;
    // Use this for initialization
    void Start()
    {
        setIsTake(false);       //預設未持球
        setIsGround(true);      //預設Player在地面
        refresh_allTakeable();  //初始時更新可撿拾物清單
        velocityText = GameObject.Find("velocityText").GetComponent<UnityEngine.UI.Text>();

    }

    // Update is called once per frame
    void Update()
    {
        animatorState();        //依據player的各種狀態，判斷現在對應的動畫狀態
        velocityTextUdate();    //更新velocity資訊
        targetBallSerach();     //尋找可以撿起的球
    }
    void takeBall()
    {
        if (isclosetBallClosetEnough()) //如果球在可撿範圍內
        {
            copyNewBall();              //將一顆新球
            putBallOnHead();            //把新球放在頭上
            DestoryOldBall();           //摧毀地上的舊球
            setIsTake(true);            //設定目前是持球狀態
        }
    }
    void throwBall()
    {
        give_newBall_Parameter();       //給予丟出去的球各項狀態
        addForceTo_newBall();           //給予丟出去的球力量值
        refresh_allTakeable();          //更新可撿的球的清單
        setIsTake(false);               //設定目前是空手狀態
    }
    public void move(string direct, float n)
    {
        clampsMoveVelocity(n);          //限制Player移動速度
        flipSpriteByDirect(direct);     //依據輸入的方向，決定是否要翻轉spriteX軸
        addForceToPlayer(direct);       //給予player移動量
        dectedBreakAnimator(direct);    //依據輸入方向，決定是否撥放煞車動作
    }

    void targetBallSerach()
    {
        if (isTake == false)
        {
            getClosetBall();
            keepClosetBallParameter();
        }
    }
    void keepClosetBallParameter()
    {
        lastclosetBall = closetBall.transform.parent;
        lastclosetBallScale = closetBall.transform.localScale;
    }
    void getClosetBall()
    {
        float dist = Mathf.Infinity;
        GameObject closetObj = allTakeable[0];
        foreach (var t in allTakeable)
        {
            if (t.GetComponent<Renderer>().isVisible)
            {
                float temp = Vector3.Distance(t.transform.position, transform.position);
                if (temp < dist)
                {
                    dist = temp;
                    closetObj = t;
                }
            }
        }
        closetBall = closetObj;

    }
    void velocityTextUdate()
    {
        velocityText.text = this.GetComponent<Rigidbody>().velocity.ToString("F1");

    }
    public void jump()
    {
        if (IsGrounded)
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 270);
    }

    public void fall()
    {
        Vector3 temp3 = this.GetComponent<Rigidbody>().velocity;
        if (temp3.y > 0)
            this.GetComponent<Rigidbody>().velocity = new Vector3(temp3.x, temp3.y * 0.5f, temp3.z);

    }
    public void pressB()
    {
        if (isTake)
        {
            throwBall();
        }
        else
        {
            takeBall();
        }
    }

    void setIsTake(bool n)
    {
        isTake = n;
    }
    void setIsGround(bool n)
    {
        IsGrounded = n;
    }
    void give_newBall_Parameter()
    {
        newBall.transform.parent = lastclosetBall;
        newBall.transform.tag = "blockTake";
        newBall.transform.localScale = lastclosetBallScale;
        newBall.AddComponent<Rigidbody>();
        newBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        newBall.AddComponent<ball>();
        decte_newBall_TypeByName();
    }
    void decte_newBall_TypeByName()
    {
        if (newBall.GetComponent<m101>())
        {
            newBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            newBall.GetComponent<m101>().enabled = true;
            newBall.GetComponent<Animator>().enabled = true;
        }
    }
    void addForceTo_newBall()
    {
        newBall.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        newBall.GetComponent<Rigidbody>().AddForce(Vector3.up * 200);
        if (this.GetComponent<SpriteRenderer>().flipX)
        {
            newBall.GetComponent<Rigidbody>().AddForce(Vector3.left * 100);
        }
        else
        {
            newBall.GetComponent<Rigidbody>().AddForce(Vector3.right * 100);
        }
    }

    void addForceTo_newBall_NoMiss()
    {
        float dx = Mathf.Abs(GameObject.Find("Basket").transform.position.x - newBall.transform.position.x);
        float dy = Mathf.Abs(GameObject.Find("Basket").transform.position.y - newBall.transform.position.y);
        float vx = dx / 2f;
        float vy = ((2 * vx * vx * dy) + (9.8f * dx * dx)) / (2 * vx * dx);
        newBall.GetComponent<Rigidbody>().velocity = new Vector3(vx, vy, 0);
    }
    void refresh_allTakeable()
    {
        allTakeable = GameObject.FindGameObjectsWithTag("blockTake");
    }

    bool isclosetBallClosetEnough()
    {
        return (Vector3.Distance(transform.position, closetBall.transform.position) < 1.5f);

    }
    void copyNewBall()
    {
        newBall = Instantiate(closetBall);
        newBall.transform.tag = "Untagged";
        Destroy(newBall.GetComponent<Rigidbody>());
        Destroy(newBall.GetComponent<ball>());
        newBall.transform.parent = transform;
    }
    void DestoryOldBall()
    {
        Destroy(closetBall);
    }
    void putBallOnHead()
    {
        if (closetBall.name == "Sphere")
        {
            newBall.name = closetBall.name;
            newBall.transform.localPosition = Vector3.zero + Vector3.up * 0.32f;
        }
        else
        {
            newBall.transform.localPosition = Vector3.zero + Vector3.up * 0.25f;
        }

    }

    void clampsMoveVelocity(float n)
    {
        n = Mathf.Abs(n);
        Vector3 temp3 = this.GetComponent<Rigidbody>().velocity;
        this.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Clamp(temp3.x, -4, 4) * n, temp3.y, temp3.z);
    }
    void flipSpriteByDirect(string d)
    {
        if (d == "left")
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        if (d == "right")
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    Vector3 getFocreByDirect(string d)
    {
        if (d == "left")
        {
            return Vector3.left;
        }
        else
        if (d == "right")
        {
            return Vector3.right;
        }
        else
        {
            return Vector3.zero;
        }
    }
    void addForceToPlayer(string direct)
    {
        Vector3 vectorDirect = getFocreByDirect(direct);
        if (IsGrounded)
        {
            this.GetComponent<Animator>().SetInteger("state", 1);
            this.GetComponent<Rigidbody>().AddForce(vectorDirect * 8);
        }
        else
        {
            this.GetComponent<Rigidbody>().AddForce(vectorDirect * 4);
        }
    }

    void dectedBreakAnimator(string direct)
    {

        if (direct == "left" && GetComponent<Rigidbody>().velocity.x > 0)
        {
            this.GetComponent<Animator>().SetInteger("state", 4);

        }
        if (direct == "right" && GetComponent<Rigidbody>().velocity.x < 0)
        {
            this.GetComponent<Animator>().SetInteger("state", 4);
        }
    }



    //animatorState: 依據player的各種狀態，判斷現在對應的動畫狀態
    void animatorState()
    {
        if (!IsGrounded)
        {
            //如果在空中，且正在往上升:2-jump
            if (GetComponent<Rigidbody>().velocity.y > 0)
            {
                GetComponent<Animator>().SetInteger("state", 2);
            }
            else
            //如果在空中，且正在往下降:3-fall
            {
                GetComponent<Animator>().SetInteger("state", 3);
            }
        }

        if (IsGrounded)
        {
            if (GetComponent<Rigidbody>().velocity.x > -0.5f
            && GetComponent<Rigidbody>().velocity.x < 0.5f
            && GetComponent<Animator>().GetInteger("state") != 5)
            {
                //如果在地面:0-idel
                GetComponent<Animator>().SetInteger("state", 0);
            }
            else
            {
                // GetComponent<Animator>().SetInteger("state", 1);
                GetComponent<Animator>().speed = 0.5f + Mathf.Abs(GetComponent<Rigidbody>().velocity.x) / 5;
            }


        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "block"
        || collision.gameObject.tag == "blockTake")
        {
            setIsGround(true);
        }
    }


    void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "block"
        || collision.gameObject.tag == "blockTake")
        {
            setIsGround(true);
        }
    }

    void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "block"
        || collision.gameObject.tag == "blockTake")
        {
            setIsGround(false);
        }
    }
}
