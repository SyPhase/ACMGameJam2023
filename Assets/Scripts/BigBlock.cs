using System.Collections;
using UnityEngine;

public class BigBlock : MonoBehaviour
{
    // Variables to set in the Inspector
    [SerializeField] float movementForce = 5f;
    [SerializeField] float maxVelocity = 150f;
    [Range(0, 10), SerializeField] int pressBuffer = 5;

    [SerializeField] AudioClip slamSFX;
    [Range(0f, 1f), SerializeField] float slamVolume = 1f;

    // private variables used by script
    int pressedSpace = 0;

    bool isCollidingWithTarget = false; // Status: are you colliding with the target?

    float missionTime = 0f;

    // Reference variables to components
    Rigidbody rb;
    AudioSource audioSource;
    Camera camera;
    SmallBlock smallBlock;
    MissionTimer missionTimer;

    // Called once when the game starts
    void Start()
    {
        // Get component references the component I want on this GameObject
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        camera = GetComponentInChildren<Camera>();
        smallBlock = FindObjectOfType<SmallBlock>(true);
        missionTimer = FindObjectOfType<MissionTimer>();
    }

    void OnEnable()
    {
        if (isCollidingWithTarget)
        {
            // Move Big Block back to avoid collision with target again
            transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            // Make movement negative to go the other way
            movementForce = -movementForce;

            // Reset values
            isCollidingWithTarget = false;
            pressedSpace = 0;
        }
    }

    void OnDisable()
    {
        // Start SmallBlock's mission timer
        missionTimer.SetMissionTimeAndStartCountdown(missionTime);
    }

    // Called each frame to get input
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Input.anyKeyDown
        {
            // will allow the force to be added pressBuffer times (pressBuffer * 0.02s)
            pressedSpace += pressBuffer;
        }

        // TODO : Remove Cheat Code
        if (Input.GetKeyDown(KeyCode.P))
        {
            missionTime = 30f;
            StartCoroutine(SwitchToSmallBlock(1f));
        }
    }

    // Called every 0.02 seconds or 50 times per second
    void FixedUpdate()
    {
        // Once collsion with target has happened, stop moving
        if (isCollidingWithTarget)
        {
            return;
        }

        // Clamp the velocity to the maxVelocity
        if (0 < pressedSpace && maxVelocity > rb.velocity.x)
        {
            rb.AddForce(movementForce * rb.mass, 0f, 0f);

            // Remove one
            pressedSpace--;

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
        else if (pressedSpace == 0) // If no input, slow the player down
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

        // Debug: print out impulse magnitude
        print(collision.impulse.magnitude);

        missionTime = collision.impulse.magnitude / 24000f ;

        StartCoroutine(SwitchToSmallBlock(5f));
    }

    IEnumerator SwitchToSmallBlock(float seconds)
    {
        // Stops the movement in FixedUpdate
        isCollidingWithTarget = true;

        // Stop engine sound
        audioSource.Stop();

        // Play slamSFX at slamVolume
        audioSource.PlayOneShot(slamSFX, slamVolume);

        // Wait 5 seconds
        yield return new WaitForSeconds(seconds);

        // Activate SmallBlock
        smallBlock.gameObject.SetActive(true);

        // Set the camera's parent to be the CameraPlaceholder on the SmallBlock
        camera.transform.parent = smallBlock.GetComponentInChildren<CameraPlaceholder>().transform;

        // Reset camera's transform for use on SmallBlock
        camera.transform.localPosition = Vector3.zero;
        camera.transform.localEulerAngles = Vector3.zero;
        camera.transform.localScale = Vector3.zero;

        gameObject.SetActive(false);
    }

    public void DisableBlock()
    {
        // Stops the movement in FixedUpdate
        isCollidingWithTarget = true;
        pressedSpace = 0;
    }
}