using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Renderer renderer;

    // Variable to store ButtonTracker
    ButtonTracker buttonTracker;

    void Start()
    {
        // Cache reference to ButtonTracker
        buttonTracker = FindObjectOfType<ButtonTracker>(true);

        // Adds one to Button count
        buttonTracker.AddButton();
    }

    void OnTriggerEnter(Collider other)
    {
        // if NOT the player, do nothing
        if (!other.CompareTag("Player"))
        {
            print("button returned...");
            return;
        }

        // Adds one to pressed Button count
        buttonTracker.ButtonWasPressed();

        // Change color of button to red
        renderer.material.color = Color.red;

        // Debug
        //print("Player hit button!");
    }
}