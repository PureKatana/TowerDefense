using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [Header("Tower Setup")]
    [Space]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10.0f;
    private bool canRotate = true;

    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected float attackDamage = 100f;

    [SerializeField] protected float attackCooldown = 2f;
    protected float lastTimeAttacked;

    protected virtual void Awake()
    {
        
    }
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

    public void EnableRotation(bool enable)
    {
        canRotate = enable;
    }

    protected virtual void RotateTowerTowardsEnemy()
    {
        if (canRotate && currentEnemy != null)
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
        List<Enemy> enemiesInRange = new List<Enemy>();
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemyCollider in enemiesColliders)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();

            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return GetClosestEnemy(enemiesInRange);
    }

    private static Transform GetClosestEnemy(List<Enemy> enemiesInRange)
    {
        Enemy closestEnemy = null;
        float minRemainingDistance = float.MaxValue;

        foreach (Enemy enemy in enemiesInRange)
        {
            float remainingDistance = enemy.GetDistanceToFinishLine();
            if (remainingDistance < minRemainingDistance)
            {
                minRemainingDistance = remainingDistance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy != null ? closestEnemy.transform : null;
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
