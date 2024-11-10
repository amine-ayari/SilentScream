using UnityEngine;
using TMPro; // Include this for TextMeshPro
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 10f; // Set the countdown time
    public GameObject objectToDisable; // Assign the object you want to disable in the inspector
    public TMP_Text timerText; // Assign a TextMeshPro Text component to display the timer

    private void Start()
    {
        PlayerPrefs.SetInt("timer", 0); // Set the PlayerPrefs variable to 0
        PlayerPrefs.Save(); // Save the PlayerPrefs
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (countdownTime > 0)
        {   
            // Update the timer text if you have a TMP_Text assigned
            if (timerText != null)
            {
                timerText.text = countdownTime.ToString("F0"); // Display as integer
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownTime--; // Decrease the countdown
        }

        // When the countdown reaches 0
        PlayerPrefs.SetInt("timer", 1); // Set the PlayerPrefs variable to 1
        PlayerPrefs.Save(); // Save the PlayerPrefs
        Debug.Log("Timer reached zero. PlayerPrefs timer set to 1."); // Log message for debugging

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false); // Disable the specified object
        }
    }
}
