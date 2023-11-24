using UnityEngine;

public class BigBlock : MonoBehaviour
{
    // Variables to set in the Inspector
    [SerializeField] float movementForce = 10f;
    [SerializeField] float maxVelocity = 3f;

    [SerializeField] AudioClip slamSFX;
    [Range(0f, 1f), SerializeField] float slamVolume = 1f;

    // private variables used by script
    float xAxis = 0f;
    float yAxis = 0f;

    bool isCollidingWithTarget = false; // Status: are you colliding with the target?

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
        // Once collsion with target has happened, stop moving
        if (isCollidingWithTarget)
        {
            return;
        }

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
            if (audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
            else
            {
                audioSource.Play();
            }
        }
        else if (xAxis == 0) // If no input, slow the player down
        {
            if (rb.velocity.x > 0)
            {
                rb.AddForce(-rb.mass, 0f, 0f);
            }
            else if (rb.velocity.x < 0)
            {
                rb.AddForce(rb.mass, 0f, 0f);
            }

            // Pause engine sound
            audioSource.Pause();
        }

        // if the x-velocity is close to zero
        if (0.5f > rb.velocity.x && -0.5f < rb.velocity.x)
        {
            // Pause engine sound
            audioSource.Pause();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // if the collision is not with the target, ignore it
        if (!collision.gameObject.CompareTag("Target"))
        {
            return;
        }

        // Stops the movement in FixedUpdate
        isCollidingWithTarget = true;

        // Stop engine sound
        audioSource.Stop();

        // Play slamSFX at slamVolume
        audioSource.PlayOneShot(slamSFX, slamVolume);

        // Debug: print out impulse magnitude
        print(collision.impulse.magnitude);
    }
}