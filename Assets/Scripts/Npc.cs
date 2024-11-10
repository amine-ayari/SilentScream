using UnityEngine;
using CandyCoded.HapticFeedback;

public class NPCStateMachine : MonoBehaviour
{
    public float detectionDistance = 10f; // Distance within which the NPC can detect the player
    public float stopDistance = 2f;        // Distance at which the NPC stops moving towards the player
    public float walkSpeed = 3f;           // Speed at which the NPC walks
    public float obstacleAvoidanceDistance = 1f; // Distance to avoid obstacles
    public float fieldOfViewAngle = 45f;   // Angle of the NPC's field of view
    public float rotationSpeed = 5f;       // Speed at which the NPC rotates to face the player

    public Animator animator;                // Reference to the Animator component
    private Transform playerTransform;       // Reference to the player's transform
    private HeartManager heartManager;       // Reference to the HeartManager script

    private float distanceToPlayer;          // Distance to the player
    private float attackCooldown = 5f;       // Cooldown duration for attacks
    private float lastAttackTime;            // Time when the last attack occurred

    private void Start()
    {
        // Find the player object tagged as "Player"
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found. Ensure there's an object tagged as 'Player' in the scene.");
        }

        // Find the HeartManager in the scene
        heartManager = FindObjectOfType<HeartManager>();
        if (heartManager == null)
        {
            Debug.LogWarning("HeartManager not found in the scene.");
        }

        // Set the initial last attack time to create a delay of attackCooldown seconds from spawn
        lastAttackTime = Time.time - attackCooldown;
    }

    private void Update()
    {
        // Ensure playerTransform is assigned
        if (playerTransform == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if the player is within detection radius
        if (distanceToPlayer <= detectionDistance)
        {
            // Check if the player is within the field of view
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < fieldOfViewAngle / 2)
            {
                Debug.Log("Player detected: " + playerTransform.name);
                MoveTowardsPlayer(); // Keep moving towards the player
                LookAtPlayer(); // Make NPC look at the player
            }
            else
            {
                Idle(); // Player is out of field of view
            }
        }
        else
        {
            Idle(); // No player detected
        }
    }

    private void MoveTowardsPlayer()
    {  
        animator.SetFloat("Speed", walkSpeed);
        animator.SetBool("IsAttacking", false); // Stop moving

        // Direction towards player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = -4; // Keep the y component as 0

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        Debug.Log("distance: " + distanceToPlayer);

        // Move the NPC towards the player, but stop before reaching
        if (distanceToPlayer > stopDistance)
        {
            // Raycast to check for obstacles
            if (Physics.Raycast(transform.position, direction, obstacleAvoidanceDistance))
            {   
                // Obstacle detected, change direction
                direction += transform.right * 0.5f; // Adjust to steer away from the obstacle
            }

            // Move towards the player
            Vector3 newPosition = transform.position + direction * walkSpeed * Time.deltaTime;

            // Ensure we do not go past the stopping distance
            float newDistanceToPlayer = Vector3.Distance(newPosition, playerTransform.position);
            if (newDistanceToPlayer <= stopDistance)
            {
                // Adjust the new position to maintain the stopping distance
                newPosition = playerTransform.position - direction * stopDistance;
                walkSpeed = 0;
                AttemptAttack(); // Attempt to attack when close enough to the player
            }

            newPosition.y = -4; // Keep the NPC's Y position at 0
            transform.position = newPosition;
        }
        else
        {
            AttemptAttack(); // Attempt to attack if already close enough
        }
    }

    private void LookAtPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Create a rotation that looks at the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Extract the current rotation and keep X and Z fixed
        Vector3 currentRotation = transform.eulerAngles;
        float targetYRotation = lookRotation.eulerAngles.y; // Get the Y rotation from the lookRotation

        // Apply only the Y rotation while keeping X and Z the same
        transform.eulerAngles = new Vector3(currentRotation.x, targetYRotation, currentRotation.z);
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0f); // Stop moving
    }

    private void AttemptAttack()
    {
        // Check if enough time has passed since the last attack
        if (Time.time - lastAttackTime >= attackCooldown)
        { 
            HapticFeedback.MediumFeedback();

            Attack(); // Call the attack method
            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    private void Attack()
    {
        animator.SetFloat("Speed", 0f); // Stop moving
        animator.SetBool("IsAttacking", true); // Set attacking animation
        // Call the function to disable the next heart
        if (heartManager != null)
        {
            heartManager.DisableNextHeart(); // Disable the next heart
        }
        else
        {
            Debug.LogWarning("HeartManager reference is not set in NPCStateMachine.");
        }
    }

    private void OnDrawGizmos()
    {
        // Draw detection distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);

        // Draw the field of view angle as a cone
        Gizmos.color = Color.green;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward * detectionDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward * detectionDistance;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, playerTransform.position); // Line to the player for visual reference
    }
}
