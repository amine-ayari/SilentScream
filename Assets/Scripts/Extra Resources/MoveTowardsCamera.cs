using UnityEngine;

public class MoveTowardsCamera : MonoBehaviour
{
    // Speed at which the object moves towards the camera's X  position
    public float moveSpeed = 2f;

    // Reference to the main camera
    private Transform cameraTransform;

    // Initialize cameraTransform
    void Start()
    {
        // Get the main camera's transform
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current position of the object
        Vector3 currentPosition = transform.position;

        // Target X position is the camera's X position
        float targetX = cameraTransform.position.x;
        float targetZ = cameraTransform.position.z;

        // Interpolate the object's X position towards the camera's X position
        float newX = Mathf.Lerp(currentPosition.x, targetX, moveSpeed * Time.deltaTime);
        float newZ = Mathf.Lerp(currentPosition.z, targetZ, moveSpeed * Time.deltaTime);
        // Update the object's position with the new X value, keeping Y and Z the same
        transform.position = new Vector3(newX, currentPosition.y, newZ);
    }
}
