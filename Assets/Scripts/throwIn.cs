using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwIn : MonoBehaviour
{
    main main;
    public int score;
    int best;
    // Use this for initialization
    void Start()
    {
        main = GameObject.Find("main").GetComponent<main>();
        best = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>().velocity.y < 0)
        {
            main.throwIn();
            if (other.GetComponent<ball>().isNiceshoot)
            {
                // main.throwIn();
                // main.throwIn();
                // main.throwIn();
                // main.throwIn();
            }

            float dist = Vector3.Distance(other.GetComponent<ball>().starPos, this.transform.position);
            Debug.Log(dist);
            if (dist > 5)
            {
                main.throwIn();
                main.throwIn();
                Debug.Log("there isNiceshoot");
            }
        }
    }

}
