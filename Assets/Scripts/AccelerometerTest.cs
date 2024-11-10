using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class AccelerometerTest : MonoBehaviour
{
    public TextMeshProUGUI accelerationText; // Reference to the TextMeshProUGUI for displaying acceleration

    private void Update()
    {
        // Get the current acceleration

        
        Vector3 acceleration = Input.acceleration;

        // Display the acceleration values
        if (accelerationText != null)
        {
            accelerationText.text = $"Acceleration:\nX: {acceleration.x:F2}\nY: {acceleration.y:F2}\nZ: {acceleration.z:F2}";
        }
    }
}
