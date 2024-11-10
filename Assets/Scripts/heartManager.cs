using UnityEngine;
using UnityEngine.UI; // Import the Unity UI namespace
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class HeartManager : MonoBehaviour
{
    public GameObject[] hearts; // Array to hold heart GameObjects
    private int currentHeartIndex = 0; // Index of the current heart to disable

    private void Start()
    {
        // Ensure all hearts are active at the start
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(true);
        }
    }

    // Public function to disable the next heart
    public void DisableNextHeart()
    {
        if (currentHeartIndex < hearts.Length)
        {
            hearts[currentHeartIndex].SetActive(false); // Disable the current heart
            currentHeartIndex++; // Move to the next heart

            // Check if all hearts are disabled
            if (currentHeartIndex >= hearts.Length)
            {
                Debug.Log("All hearts are disabled. Game Over!");
                SceneManager.LoadScene("end"); // Load the "end" scene
            }
        }
    }
}
