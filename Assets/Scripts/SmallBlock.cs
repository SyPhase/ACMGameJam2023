using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBlock : MonoBehaviour
{
    // Variables to set in the Inspector
    [SerializeField] float movementForce = 25f;
    [SerializeField] float maxVelocity = 7.5f;
    [SerializeField] float jumpForce = 200f;

    // private variables used by script
    float xAxis = 0f;
    float yAxis = 0f;
    bool jumping = false;

    int touchingGround = 0;

    // Reference variables to components
    Rigidbody rb;
    ButtonTracker buttonTracker;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        buttonTracker = GetComponent<ButtonTracker>();
    }

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        if (!jumping && rb.velocity.y < 0.1f && rb.velocity.y > -0.1f)
        {
            jumping = Input.GetKeyDown(KeyCode.Space);
        }
    }

    void FixedUpdate()
    {
        // Normalize the x and y input
        Vector2 inputMovement = new Vector2(xAxis, yAxis).normalized;

        // Only use x and z for the magnitude check, don't include jumping
        Vector2 walkingVelocity = new Vector2(rb.velocity.x, rb.velocity.z);

        // Don't allow walking speed to go over maxVelocity
        if (maxVelocity > walkingVelocity.magnitude)
        {
            rb.AddForce(inputMovement.x * movementForce * rb.mass, 0f, inputMovement.y * movementForce * rb.mass);
        }

        // Jump
        if (touchingGround > 0)
        {
            if (jumping && rb.velocity.y < 0.1f)
            {
                rb.AddForce(0f, jumpForce * rb.mass, 0f);

                // Reset jump
                jumping = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target") && buttonTracker.AreAllButtonsPressed())
        {
            // Debug: Win!
            //print("Win!");
        }

        Vector3 norm = collision.GetContact(0).normal;

        if (norm == transform.forward || norm == transform.right || norm == -transform.forward || norm == -transform.right)
        {
            print("Touching wall!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NotGround"))
        {
            return;
        }

        touchingGround++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NotGround"))
        {
            return;
        }

        touchingGround--;
    }
}