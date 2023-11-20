using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] float movementForce = 10f;
    [SerializeField] float maxVelocity = 3f;

    float xAxis = 0f;
    float yAxis = 0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");

        if (rb.velocity.magnitude < maxVelocity)
        {
            rb.AddForce(xAxis * movementForce * rb.mass, yAxis * movementForce * rb.mass, 0);
        }
    }
}