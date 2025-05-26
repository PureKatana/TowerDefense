using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [Header("Tower Setup")]
    [SerializeField] private Transform towerHead;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask whatIsEnemy;

    private void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemyWithinRange();
            return;
        }

        if (Vector3.Distance(transform.position, currentEnemy.position) > attackRange)
        {
            currentEnemy = null;
        }

        RotateTowerTowardsEnemy();
    }

    private void RotateTowerTowardsEnemy()
    {
        if (currentEnemy != null)
        {
            Vector3 direction = currentEnemy.position - towerHead.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private Transform FindRandomEnemyWithinRange()
    {
        List<Transform> enemiesInRange = new List<Transform>();
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemyCollider in enemiesColliders)
        {
            enemiesInRange.Add(enemyCollider.transform);
        }

        int randomIndex = Random.Range(0, enemiesInRange.Count);

        return enemiesInRange.Count > 0 ? enemiesInRange[randomIndex] : null;
    }

    private void OnDrawGizmos()
    {
        if (towerHead != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
