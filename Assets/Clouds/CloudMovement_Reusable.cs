using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement_Reusable : MonoBehaviour
{

    public GameObject G_end;

    public float speed = 1.0f;
    public Transform T_end;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        T_end = G_end.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Work out the direction we need to move in:
        Vector3 displacement = T_end.position - transform.position;
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
