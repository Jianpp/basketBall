using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class updown : MonoBehaviour
{
    public Vector3 startPos;
    public bool onThis;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Rigidbody>().freezeRotation = true;
        this.GetComponent<Rigidbody>().useGravity = false;
        onThis = false;
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!onThis && this.transform.position.y > startPos.y)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.down * 2.0f;
            // transform.position -= Vector3.up * 0.03f;
        }
        if (!onThis && this.transform.position.y < startPos.y)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }
    void OnCollisionStay(Collision other)
    {
        onThis = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.up * 2.0f;
    }
    void OnCollisionExit(Collision other)
    {
        onThis = false;
    }
}
