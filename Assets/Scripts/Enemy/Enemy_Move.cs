using AudioSystem;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour {
    public NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component
    public float speedRun = 9; // Speed at which the enemy moves
    public float obstacleAvoidanceRange = 5f; // Distance within which the enemy avoids obstacles
    EnemyController enemyController;
   

    Transform player; // Reference to the player's transform
    bool m_CaughtPlayer; // Flag indicating whether the player has been caught

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to this object
        navMeshAgent.speed = speedRun; // Set the speed of the NavMeshAgent
        navMeshAgent.autoBraking = false; // Disable auto braking for smoother navigation

        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object based on tag
    }

    void Update() {
        //if (!m_CaughtPlayer && player != null) {
        ChasePlayer(); // Call method to chase the player
        //}
    }

    void ChasePlayer() {
        navMeshAgent.SetDestination(player.position); // Set the destination of the NavMeshAgent to the player's position

        // Check for obstacles along the path to the player
        if (navMeshAgent.remainingDistance <= obstacleAvoidanceRange && !navMeshAgent.pathPending) {
            Vector3 direction = (player.position - transform.position).normalized; // Calculate direction to player
            Vector3 targetPosition = transform.position + direction * obstacleAvoidanceRange; // Calculate target position

            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, obstacleAvoidanceRange, NavMesh.AllAreas)) {
                navMeshAgent.SetDestination(hit.position); // Set new destination to avoid the obstacle
            }
        }
    }
   
}
