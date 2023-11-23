using UnityEngine;

public class BigBlock : MonoBehaviour
{
    // Variables to set in the Inspector
    [SerializeField] float movementForce = 10f;
    [SerializeField] float maxVelocity = 3f;

    // private variables used by script
    float xAxis = 0f;
    float yAxis = 0f; //yAxis * movementForce * rb.mass

    // Reference variables to components
    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        // Get component references the component I want on this GameObject
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Get the Input
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");

        // Use x or y, whichever's absolute value is larger
        if (Mathf.Abs(xAxis) < Mathf.Abs(yAxis))
        {
            xAxis = yAxis;
        }

        // Clamp the velocity to the maxVelocity
        if ((0 < xAxis && maxVelocity > rb.velocity.x) || (0 > xAxis && -maxVelocity < rb.velocity.x))
        {
            rb.AddForce(xAxis * movementForce * rb.mass, 0f, 0f);

            // Play engine sound
            audioSource.Play();
        }

        // If no input, slow the player down
        if (xAxis == 0)
        {
            if (rb.velocity.x > 0)
            {
                rb.AddForce(-rb.mass, 0f, 0f);
            }
            else if (rb.velocity.x < 0)
            {
                rb.AddForce(rb.mass, 0f, 0f);
            }

            // Stop engine sound
            audioSource.Pause();
        }
    }
}