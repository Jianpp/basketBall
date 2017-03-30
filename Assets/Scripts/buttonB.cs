using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class buttonB : MonoBehaviour
{
    public bool touch;
    bool pressB;
    player player;

    // Use this for initialization
    GameObject hitUIObject;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<player>();
        pressB = false;
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
                    if (hitUIObject.name == this.name && !pressB)
                    {
                        touch = true;
                        player.pressB();
                        pressB = true;
                    }
                }
            }
        }
        else
        {
            if (touch)
            {
                // GameObject.Find("player").GetComponent<player>().fall();
                touch = false;
            }
            else
            {
                pressB = false;
            }

        }

    }
}
