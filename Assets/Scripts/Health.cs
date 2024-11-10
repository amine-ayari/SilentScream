using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;      // Maximum health
    public int currentHealth;        // Current health
    public Image healthBar;          // Health bar UI element
    public int enemyID;              // Unique ID for each enemy

    void Start()
    {
        // Set current health to max at the start
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // Call this method when the object collides with a bullet
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Clamp health to a minimum of 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar
        UpdateHealthBar();

        // Check if health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Update the health bar based on current health
    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    // Call this method when health reaches zero
    void Die()
    {
        // Add behavior for when the object dies (e.g., destroy object, respawn, etc.)
        Debug.Log($"Enemy {enemyID} is dead!");
        Destroy(gameObject); // Example: destroy the object
    }

    // Detect collision with the bullet using OnTriggerEnter (for trigger colliders)
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(10); // Reduce health by 10 when hit by a bullet
        }
    }
}
