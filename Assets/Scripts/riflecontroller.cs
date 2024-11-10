using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class ARFireScript : MonoBehaviour
{
    // Reference to the projectile prefab
    public GameObject projectilePrefab;
    
    // Reference to the rifle (or the place where the projectile spawns)
    public Transform rifleTip;

    // The speed at which the projectile will be fired
    public float projectileSpeed = 1000f;

    void Update()
    {
        // Check for touchscreen input using the new Input System
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Detect the touch phase (equivalent to "TouchPhase.Began" in the old system)
            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                FireProjectile();
            }
        }

        // Optional: Also allow left mouse click for testing in the editor
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        // Instantiate the projectile at the rifle's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, rifleTip.position, rifleTip.rotation);

        // Add force to the projectile to make it move forward
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(rifleTip.forward * projectileSpeed);
        }
    }
}
