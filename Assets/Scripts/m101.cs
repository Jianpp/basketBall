using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m101 : MonoBehaviour
{

    // Use this for initialization
    public bool IsGrounded;
    Vector3 goPos;
    Transform playerTransform;
    float speed;
    void Start()
    {
        GetComponent<Animator>().speed = 0.1f;
        speed = 2.0f;
        playerTransform = GameObject.Find("player").transform;
        goPos = this.transform.position;
        IsGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Renderer>().isVisible)
        {
            // ai();
            // movement();
        }

    }

    void ai()
    {
        if (Mathf.Abs(playerTransform.position.y - this.transform.position.y) < 3f)
        {
            var dist = Vector3.Distance(this.transform.position, playerTransform.position);
            if (dist < 5)
            {
                goPos = playerTransform.position;
                if (dist < 1)
                {
                    goPos = this.transform.position;
                }
            }
        }
        else
        {
            goPos = this.transform.position;

        }

    }

    void movement()
    {
        if (IsGrounded)
        {
            Vector3 dir = Vector3.zero;
            var dist = Vector3.Distance(this.transform.position, goPos);
            if (dist < 4)
            {
                dir = (goPos - transform.position).normalized * speed;
            }
            GetComponent<Rigidbody>().velocity = new Vector3(dir.x, 0, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //碰到block時
        if (collision.gameObject.tag == "block")
        {
            IsGrounded = true;
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "blockTake")
        {
            if (collision.transform.position.y > this.transform.position.y)
            {
                var temp = collision.gameObject.GetComponent<Rigidbody>().velocity.y;
                if (temp <= 1)
                {
                    temp = 0;
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 250 + new Vector3(0, temp, 0));
                    GetComponent<Animator>().SetInteger("state", 9);
                }
            }
        }
    }

    //OnCollisionEnter:當player持續碰到其他collision時(重複性)...
    void OnCollisionStay(Collision collision)
    {
        //碰到block時
        if (collision.gameObject.tag == "block")
        {
            IsGrounded = true;
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "blockTake")
        {
            if (collision.transform.position.y > this.transform.position.y)
            {
                GetComponent<Animator>().SetInteger("state", 9);
            }
        }
        if (collision.gameObject.tag == "emeny")
        {
            goPos = this.transform.position;
        }
    }

    //OnCollisionEnter:當player離開碰到的collision時(一次性)...
    void OnCollisionExit(Collision collision)
    {
        //當離開block時
        if (collision.gameObject.tag == "block")
        {
            IsGrounded = false;
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "blockTake")
        {
            if (collision.transform.position.y > this.transform.position.y)
            {
                GetComponent<Animator>().SetInteger("state", 1);
            }
        }
    }
}
