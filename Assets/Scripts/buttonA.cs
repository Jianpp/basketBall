using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class buttonA : MonoBehaviour
{
    public bool touch;
    bool pressA;

    // Use this for initialization
    GameObject hitUIObject;
    player player;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<player>();
        pressA = false;
        touch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            if (EventSystem.current.IsPointerOverGameObject())
            {
                hitUIObject = EventSystem.current.currentSelectedGameObject;
                if (hitUIObject)
                {
                    if (hitUIObject.name == this.name && !pressA)
                    {
                        touch = true;
                        player.jump();
                        pressA = true;
                    }
                }
            }
        }
        else
        {
            if (touch)
            {
                player.fall();
                touch = false;
            }
            else
            {
                pressA = false;
            }

        }

    }
}
