using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class ShakeMeshSwitcher : MonoBehaviour
{
    public MeshFilter meshFilter; // The MeshFilter component to modify
    public Mesh[] meshes; // Array to hold the meshes to switch between
    private int currentMeshIndex = 0; // Index of the current mesh
    private float shakeThreshold = 5f; // Threshold for detecting a shake
    private float lastShakeTime = 0.0f; // Timestamp of the last shake

    public AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Ensure the AudioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        DetectShake();
    }

    private void DetectShake()
    {
        // Check if the device is being shaken
        if (Input.acceleration.sqrMagnitude > shakeThreshold)
        {
            float currentTime = Time.time;

            if (currentTime - lastShakeTime > 0.5f) // Adjust the time to avoid multiple shakes
            {
                ChangeMesh();
                lastShakeTime = currentTime; // Update the last shake time
            }
        }
    }

    private void ChangeMesh()
    {
        if (meshes.Length > 0) // Check if there are any meshes
        {
            currentMeshIndex = (currentMeshIndex + 1) % meshes.Length; // Cycle through the meshes
            meshFilter.mesh = meshes[currentMeshIndex]; // Set the new mesh

            // Play the sound effect when the mesh changes
            if (audioSource != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource not assigned or missing.");
            }
        }
    }
}
