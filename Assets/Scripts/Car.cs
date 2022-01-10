using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{   

    // Public Variables:
    public List<Transform> wps; // Store the transforms of the waypoints.
    public List <Transform> route; // Store the routes.
    public int routeNumber = 0;
    public int targetWP = 0;
    public float speed = 0.0f;
    public float speedLimit = 20;
    public float accelerationRate = 4.0f;
    public float safetyCheck = 5f;
    
    private Rigidbody rb;
    public bool isAccelerating;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // We are going to populate the list using the watpoints objects. 
        wps = new List<Transform>();
        GameObject wp;

        // Add the first waypoint:
        wp = GameObject.Find("Car_WP1");
        wps.Add(wp.transform);

        // Add the second waypoint:
        wp = GameObject.Find("Car_WP2");
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP3");
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP4");
        wps.Add(wp.transform);
        
        speed = 0f;
        isAccelerating = true;

        SetRoute();

    }
    
    void SetRoute() {
        routeNumber = Random.Range(0, 2);
        
        // Set the route waypoints:
        if (routeNumber == 0) {
            route = new List<Transform> { wps[0], wps[1] };
        } else if (routeNumber == 1) {
            route = new List<Transform> { wps[2], wps[3] };
        }

        // Initialise the position and waypoint counter:
        transform.position = new Vector3(route[0].position.x, 0.0f, route[0].position.z);
        targetWP = 1; // Store the index of the next waypoint.
        speed = 0f;
        isAccelerating = true;
    }

    Transform getDestination() {
        Transform newDestination = route[targetWP];
        return newDestination;
    }


    void setSpeed() {
        if (isAccelerating == false) {
            speed -= accelerationRate * Time.deltaTime;
            if (speed <= 0f) {
                speed = 0f;
                safetyCheck -= Time.deltaTime;
                if (safetyCheck <= 0f) {
                    isAccelerating = true;
                }
            }
        } else {
            speed += accelerationRate * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0f, speedLimit); 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Store the waypoint in which it is headed:
        Transform destination = getDestination();
        Vector3 displacement = destination.position - transform.position;
        float distance = displacement.magnitude;
        if (distance < 0.5f) {
            SetRoute();
        }

        setSpeed();

        // Calculate the velocity:
        Vector3 velocity = displacement;
        velocity.Normalize();
        velocity *= speed;

        // Rotate the car: 
        transform.rotation = Quaternion.LookRotation(destination.position);
        
        // Apply the velocity:
        Vector3 newPosition = transform.position;
        newPosition += velocity * Time.deltaTime;
        rb.MovePosition(newPosition);   
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("CRASH CRASH CRASH!!!!!");

        // We need to slow down:
        isAccelerating = false;
        safetyCheck = 5f;
        accelerationRate = 20f;
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("False ALARM!");
        if (speed == 0) {
            isAccelerating = true;
        }
    }
}
