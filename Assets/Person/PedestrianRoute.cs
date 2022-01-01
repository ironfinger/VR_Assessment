using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianRoute : MonoBehaviour
{
    // Public Variables:
    public List<Transform> wps; // Store the transforms of the waypoints.
    public List<Transform> route; // Store the routes.
    public int routeNumber = 0; // Store the current route.
    public int targetWP = 0; // Store the waypoint the pedestrian is heading towards.
    public float dist;
    public bool go = false;
    public float initialDelay;

    // Private Variables:
    private Rigidbody rb; // Create a variable to store the rigid body.

    // Start is called before the first frame update
    void Start()
    {   

        // Get the rigid body component:
        rb = GetComponent<Rigidbody>();

        // We are going to populate the list using the waypoint objects.
        wps = new List<Transform>();
        GameObject wp;

        // Find and add the waypoints to the wps list:
        wp = GameObject.Find("WP1");
        wps.Add(wp.transform);

        wp = GameObject.Find("WP2");
        wps.Add(wp.transform);

        wp = GameObject.Find("WP3");
        wps.Add(wp.transform);
        
        wp = GameObject.Find("WP4");
        wps.Add(wp.transform);

        wp = GameObject.Find("WP5");
        wps.Add(wp.transform);

        wp = GameObject.Find("WP6");
        wps.Add(wp.transform);

        wp = GameObject.Find("WP7");
        wps.Add(wp.transform);

        wp = GameObject.Find("WP8");
        wps.Add(wp.transform);

        // Call the set route function:
        SetRoute();

        initialDelay = Random.Range(2.0f, 12.0f);
        transform.position = new Vector3(0.0f, -5.0f, 0.0f);
    }

    void SetRoute() 
    {
        //randomise the next route
        routeNumber = Random.Range(0, 12);

        //set the route waypoints
        if (routeNumber == 0) route = new List<Transform> { wps[0], wps[4], wps[5], wps[6] };
        else if (routeNumber == 1) route = new List<Transform> { wps[0], wps[4], wps[5], wps[7] };
        else if (routeNumber == 2) route = new List<Transform> { wps[2], wps[1], wps[4], wps[5], wps[6] };
        else if (routeNumber == 3) route = new List<Transform> { wps[2], wps[1], wps[4], wps[5], wps[7] };
        else if (routeNumber == 4) route = new List<Transform> { wps[3], wps[4], wps[5], wps[6] };
        else if (routeNumber == 5) route = new List<Transform> { wps[3], wps[4], wps[5], wps[7] };
        else if (routeNumber == 6) route = new List<Transform> { wps[6], wps[5], wps[4], wps[0] };
        else if (routeNumber == 7) route = new List<Transform> { wps[6], wps[5], wps[4], wps[3] };
        else if (routeNumber == 8) route = new List<Transform> { wps[6], wps[5], wps[4], wps[1], wps[2] };
        else if (routeNumber == 9) route = new List<Transform> { wps[7], wps[5], wps[4], wps[0] };
        else if (routeNumber == 10) route = new List<Transform> { wps[7], wps[5], wps[4], wps[3] };
        else if (routeNumber == 11) route = new List<Transform> { wps[7], wps[5], wps[4], wps[1], wps[2] };

        //initialise position and waypoint counter
        transform.position = new Vector3(route[0].position.x, 0.0f,
        route[0].position.z);
        targetWP = 1;
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        // Counts the delay to zero then allows the execution of the first route.
        if (!go) {
            initialDelay -= Time.deltaTime;
            
            if (initialDelay <= 0.0f) {
                go = true;
                SetRoute();
            }
            else return;
        }

        // Work Out the direction we need to move in to get to the next waypoint:
        Vector3 displacement = route[targetWP].position - transform.position; //Computes the vector from the pedestrian position to the target position.
        displacement.y = 0; // Set the y component to zero as we are only interested in the x/z movement.
        float dist = displacement.magnitude; // Store the distance from the pedestrian to the target as the magnitude of the vector.

        // Check if the pedestrian has eached a waypoint:
        if (dist < 0.1f) {
            
            targetWP++;

            // If target wp is greater than the route count then the rotue is now complete:
            if (targetWP >= route.Count) {
                SetRoute();
                return;
            }
        }

        // Calculate velocity for this frame:
        Vector3 velocity = displacement;
        velocity.Normalize();
        velocity *= 2.5f;

        // Apply the velocity:
        Vector3 newPosition = transform.position;
        newPosition += velocity * Time.deltaTime;
        rb.MovePosition(newPosition);

        // Align velocity:
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, velocity, 10.0f * Time.deltaTime, 0f);
        Quaternion rotation = Quaternion.LookRotation(desiredForward);
        rb.MoveRotation(rotation);
    }
}
