using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud1 : MonoBehaviour
{

    // Need to get the waypoint end transform:
    public Transform end; // Store the end of the clouds journey.
    public float speed = 1.0f;

    private Rigidbody rb; // Create the rigid body variable.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        GameObject GO_end = GameObject.Find("CWP1_End");
        end = GO_end.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Work out the direction we need to move in:
        Vector3 displacement = end.position - transform.position;
        displacement.y = 0;
        float distance = displacement.magnitude;

        // Check if the cloud has reached its waypoint:
        if (distance < 0.1f) {
            Debug.Log("Done");
        }

        // Calculate the velocity:
        Vector3 velocity = displacement;
        velocity.Normalize();
        velocity *= speed;

        // Apply the velocity:
        Vector3 newPosition = transform.position;
        newPosition += velocity * Time.deltaTime;
        rb.MovePosition(newPosition);

    }
}
