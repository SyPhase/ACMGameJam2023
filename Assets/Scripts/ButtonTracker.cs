using UnityEngine;

public class ButtonTracker : MonoBehaviour
{
    int totalButtons = 0;
    int pressedButtons = 0;

    public void AddButton()
    {
        totalButtons++;
    }

    public void ButtonWasPressed()
    {
        pressedButtons++;
    }
    
    // Returns true if each button is pressed
    public bool AreAllButtonsPressed()
    {
        if (pressedButtons >= totalButtons)
        {
            return true;
        }

        // else
        return false;
    }
}