using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class SceneLoader : MonoBehaviour
{
    public Button loadSceneButton; // Reference to the Button in the scene

    private void Start()
    {
        // Assign the LoadSampleScene method to the button's onClick event
        loadSceneButton.onClick.AddListener(LoadSampleScene);
    }

    // Method to load the "SampleScene" scene
    private void LoadSampleScene()
    {
        SceneManager.LoadScene("main");
    }
}
