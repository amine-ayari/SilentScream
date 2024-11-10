using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    // Reference to the button
    public Button navigateButton;

    // Name of the scene to load
    private string mainMenuSceneName = "Main Menu";

    void Start()
    {
        // Ensure navigateButton is assigned, then add a listener
        if (navigateButton != null)
        {
            navigateButton.onClick.AddListener(OnNavigateButtonClick);
        }
    }

    // Method called when the button is clicked
    void OnNavigateButtonClick()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
