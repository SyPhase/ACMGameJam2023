using System.Collections;
using UnityEngine;

// This GameObject MUST start Disabled in the inspector
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
    bool isDeactivated = false;

    // tracks the ground in contact
    int touchingGround = 0;

    // Reference variables to components
    Rigidbody rb;
    ButtonTracker buttonTracker;
    Camera camera;
    BigBlock bigBlock;
    MissionTimer missionTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        buttonTracker = GetComponent<ButtonTracker>();
        camera = FindObjectOfType<Camera>();
        bigBlock = FindObjectOfType<BigBlock>(true);
        missionTimer = FindObjectOfType<MissionTimer>();
    }

    void OnDisable()
    {
        // Start BigBlock's mission timer
        missionTimer.SetMissionTimeAndStartCountdown(16.5f);
    }

    void Update()
    {
        if (isDeactivated)
        {
            return;
        }

        // Get input
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        // If NOT already jumping and y-velocity near zero
        if (!jumping && rb.velocity.y < 0.1f && rb.velocity.y > -0.1f)
        {
            // Check for jump input
            jumping = Input.GetKeyDown(KeyCode.Space);
        }

        // Uncomment for "Cheat Code" to skip section
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(SwitchToSmallBlock(1f));
        }*/
    }

    void FixedUpdate()
    {
        // Normalize the x and y input
        Vector2 inputMovement = new Vector2(xAxis, yAxis).normalized;

        // Add force depending on movement input
        rb.AddForce(inputMovement.x * movementForce * rb.mass, 0f, inputMovement.y * movementForce * rb.mass);

        // Don't allow walking speed (magnitude of x and z) to go over maxVelocity
        Vector3 clampedVelocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0f, rb.velocity.z), maxVelocity);

        // Set velocity to clampedVelocity with y velocity in tact
        rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);

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

        // If no x-axis input
        if (xAxis == 0)
        {
            // if x-velocity is close to zero
            if (rb.velocity.x > -0.5f && rb.velocity.x < 0.5f)
            {
                // Set x-velocity to zero
                rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
            }
            else
            {
                // add force opposite the direction of motion
                rb.AddForce(Mathf.Sign(rb.velocity.x) * movementForce * -rb.mass, 0f, 0f);
            }
        }

        // If no y-axis input
        if (yAxis == 0)
        {
            // if z-velocity is close to zero
            if (rb.velocity.z > -0.5f && rb.velocity.z < 0.5f)
            {
                // Set z-velocity to zero
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
            }
            else
            {
                // add force opposite the direction of motion
                rb.AddForce(0f, 0f, Mathf.Sign(rb.velocity.z) * movementForce * -rb.mass);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NotGround"))
        {
            return;
        }

        // Increment when touching Ground
        touchingGround++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NotGround"))
        {
            return;
        }

        // Decrement when stopped touching Ground
        touchingGround--;
    }

    void OnCollisionEnter(Collision collision)
    {
        // If collision is with target and all the buttons are pressed
        if (collision.gameObject.CompareTag("Target") && buttonTracker.AreAllButtonsPressed())
        {
            // Debug: Win!
            //print("Win!");

            missionTimer.StopTimer();

            StartCoroutine(SwitchToSmallBlock(5f));
        }
    }

    IEnumerator SwitchToSmallBlock(float seconds)
    {
        // Wait 5 seconds
        yield return new WaitForSeconds(seconds);

        // Activate BigBlock
        bigBlock.gameObject.SetActive(true);

        // Set the camera's parent to be the CameraPlaceholder on the SmallBlock
        camera.transform.parent = bigBlock.GetComponentInChildren<CameraPlaceholder>().transform;

        // Reset camera's transform for use on SmallBlock
        camera.transform.localPosition = Vector3.zero;
        camera.transform.localEulerAngles = Vector3.zero;
        camera.transform.localScale = Vector3.zero;

        gameObject.SetActive(false);
    }

    public void DisableBlock()
    {
        // Stops getting input
        isDeactivated = true;

        xAxis = 0;
        yAxis = 0;
        jumping = false;
    }
}