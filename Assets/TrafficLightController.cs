using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script will:
- Locate and save each of the three traiffic light object (transforms) + Their child objects -> corresponding lights in public vars.
- Controls the stop and go of the lights (this will be represented by two states), an integer will be used to control the state.
- There will be a timer var to control the duration of the states.
*/

public class TrafficLightController : MonoBehaviour
{
    public Transform t1;
    public Transform t2;
    public Transform t3;

    public GameObject t1green;
    public GameObject t1red;
    public GameObject t2green;
    public GameObject t2red;
    public GameObject t3green;
    public GameObject t3red;
    public float stateTimer;
    public int state;


    // Start is called before the first frame update
    void Start()
    {
        // Find the transforms of the lights:
        t1 = transform.Find("TL1");
        t2 = transform.Find("TL2");
        t3 = transform.Find("TL3");

        // Find the light game objects:
        t1green = t1.Find("Green light").gameObject;
        t1red = t1.Find("Red light").gameObject;
        t2green = t2.Find("Green light").gameObject;
        t2red = t2.Find("Red light").gameObject;
        t3green = t3.Find("Green light").gameObject;
        t3red = t3.Find("Red light").gameObject;


        // Initialise the timers:
        stateTimer = 10.0f;
        SetState(1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stateTimer <= 0f) {
            if (state == 0) SetState(1);
            else SetState(0);
            stateTimer = 10.0f;
        }

        stateTimer -= Time.deltaTime;
    }

    void SetState(int c)
    {
        state = c;
        if (c == 1)
        {
            t1green.active = true;
            t1red.active = false;
            t2green.active = false;
            t2red.active = true;
            t3green.active = false;
            t3red.active = true;
        }
        else
        {
            t1green.active = false;
            t1red.active = true;
            t2green.active = true;
            t2red.active = false;
            t3green.active = true;
            t3red.active = false;
        }
    }
}
