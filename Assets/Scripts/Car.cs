using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{   

    // Public Variables:
    public List<Transform> wps; // Store the transforms of the waypoints.
    public List <Transform> route; // Store the routes.
    public int routeNumber = 0; // Stores the route that the car is on.
    public int targetWP = 0; // Stores the target waypoint.
    public float speed = 0.0f; // Stores the current speed.
    public float speedLimit = 20; // Stores the speed limit.
    public float accelerationRate = 4.0f; // Stores the acceleration rate.
    public float safetyCheck = 5f; // The amount of time a car will waint until it starts to move again.
    
    public List <GameObject> redLights; // Stores the red lights of the traffic lights.
    public int routeLight;

    public Transform trafficLight;

    // Private Variables:
    private Rigidbody rb;
    public bool isAccelerating;

    public bool pedestrianNearby = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // We are going to populate the list using the watpoints objects. 
        wps = new List<Transform>();
        GameObject wp;

        // Add the first waypoint:
        wp = GameObject.Find("Car_WP1"); // 0
        wps.Add(wp.transform);

        // Add the second waypoint:
        wp = GameObject.Find("Car_WP2"); // 1
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP3"); // 2 
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP4"); // 3
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP5"); // 4
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP6"); // 5
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP7"); // 6
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP8"); // 7
        wps.Add(wp.transform);

        wp = GameObject.Find("Car_WP9"); // 8
        wps.Add(wp.transform);

        
        // We are going to get the traffic light red lights:
        trafficLight = GameObject.Find("TL1").transform;
        redLights.Add(trafficLight.Find("Red light").gameObject);

        trafficLight = GameObject.Find("TL2").transform;
        redLights.Add(trafficLight.Find("Red light").gameObject);

        trafficLight = GameObject.Find("TL3").transform;
        redLights.Add(trafficLight.Find("Red light").gameObject);

        speed = 0f;
        isAccelerating = true;

        SetRoute();

    }
    
    void SetRoute() {
        routeNumber = 0;
        
        // Set the route waypoints:
        if (routeNumber == 0) {
            route = new List<Transform> { wps[0], wps[1] };
            routeLight = 2;
        } else if (routeNumber == 1) {
            route = new List<Transform> { wps[2], wps[3] };
            routeLight = 1;
        } else if (routeNumber == 2) {
            route = new List<Transform> { wps[0], wps[4], wps[5] };
            routeLight = 2;
        } else if (routeNumber == 3) {
            route = new List<Transform> { wps[6], wps[7], wps[8] };
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
                    safetyCheck = 5f;
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
        displacement.y = 0;
        float distance = displacement.magnitude;
        
        // Check if the car has reahed a waypoint:
        if (distance < 0.5f) {
            targetWP++;

            if (targetWP >= route.Count) {
                SetRoute();
                return;
            }
        }

        setSpeed();

        // Traffic light check:
        GameObject currentLight = redLights[routeLight];
        Transform TcurrentLight = currentLight.transform;
        Vector3 lightDisplacement = TcurrentLight.position - transform.position;
        float lightDistance = lightDisplacement.magnitude;

        // if (redLights[routeLight].activeInHierarchy == true && lightDistance <= 15f) {
        //     isAccelerating = false;
        //     safetyCheck = 0f;
        //     accelerationRate = 20f;
        // } else if (redLights[routeLight].activeInHierarchy != true) {
        //     isAccelerating = true;
        //     accelerationRate = 4.0f;
        // }

        // Calculate the velocity:
        Vector3 velocity = displacement;
        velocity.Normalize();
        velocity *= speed;
        
        // Apply the velocity:
        Vector3 newPosition = transform.position;
        newPosition += velocity * Time.deltaTime;
        rb.MovePosition(newPosition);

        // Alight the velocity:
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, velocity, 10.0f * Time.deltaTime, 0f);
        Quaternion rotation = Quaternion.LookRotation(desiredForward);
        rb.MoveRotation(rotation);   
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pedestrian") {
            Debug.Log("Hit the pedestrian");
            isAccelerating = false;
            safetyCheck = 5f;
            accelerationRate = 25f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("False ALARM!");
        if (speed == 0) {
            isAccelerating = true;
        }
    }
}
