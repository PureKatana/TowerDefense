using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [Header("Tower Setup")]
    [Space]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10.0f;

    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;

    [SerializeField] protected float attackCooldown = 2f;
    protected float lastTimeAttacked;

    protected virtual void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemyWithinRange();
            return;
        }

        if (CanAttackEnemy())
        {
            AttackEnemy();
        }


        if (Vector3.Distance(transform.position, currentEnemy.position) > attackRange)
        {
            currentEnemy = null;
        }

        RotateTowerTowardsEnemy();
    }

    protected virtual void AttackEnemy()
    {
        //Debug.Log("Attacking enemy at : " + Time.time);
    }

    protected bool CanAttackEnemy()
    {
        if (Time.time >= lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    protected virtual void RotateTowerTowardsEnemy()
    {
        if (currentEnemy != null)
        {
            Vector3 direction = currentEnemy.position - towerHead.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    protected Vector3 GetDirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.position - startPoint.position).normalized;
    }

    protected Transform FindRandomEnemyWithinRange()
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

    protected virtual void OnDrawGizmos()
    {
        if (towerHead != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
