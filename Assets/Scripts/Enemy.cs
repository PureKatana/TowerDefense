using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private NavMeshAgent agent;

    [Header("Enemy Settings")]
    [Space]
    public float health = 100.0f;

    [Header("Movement Settings")]
    [Space]
    [SerializeField] private float turnSpeed = 10.0f;
    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        // The avoidance priority is set to a value based on the speed of the agent. Higher speed means lower priority.
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10.0f);
    }

    private void Start()
    {
        waypoints = FindFirstObjectByType<WaypointsManager>().Waypoints;
    }

    private void Update()
    {
        FaceTarget(agent.steeringTarget);
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetMaxWaypoint());
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {
        // Calculate the direction from current position to the target position
        Vector3 directionTarget = newTarget - transform.position;

        directionTarget.y = 0; // Keep the y component zero to avoid tilting

        // Create a rotation that looks in the direction of the target
        Quaternion targetRotation = Quaternion.LookRotation(directionTarget);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 GetMaxWaypoint()
    {
        if (waypointIndex >= waypoints.Length)
        {
            return transform.position;
        }

        Vector3 targetPoint = waypoints[waypointIndex].position;
        waypointIndex++;

        return targetPoint;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
