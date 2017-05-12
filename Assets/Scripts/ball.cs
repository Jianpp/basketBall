using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    public bool isNiceshoot;

    public Vector3 starPos;
    // Use this for initialization
    void Start()
    {
        isNiceshoot = true;
        starPos = new Vector3(transform.position.x, 0, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
		//杜子俄拉
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name != "player")
            isNiceshoot = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "player")
            isNiceshoot = false;
    }
}
