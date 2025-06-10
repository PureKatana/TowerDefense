using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Enemy currentEnemy;

    [Header("Tower Setup")]
    [Space]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10.0f;
    private bool canRotate = true;
    [Space]
    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected float attackDamage = 100f;
    [SerializeField] protected float attackCooldown = 2f;
    protected float lastTimeAttacked;
    [Space]
    [SerializeField] protected EnemyType enemyPriorityType = EnemyType.None;
    [Space]
    [Tooltip("Enabling this will make the tower change target between attacks")]
    [SerializeField] private bool dynamicTargetChange = true;
    private float targetCheckInterval = 0.1f;
    private float lastTargetCheckTime = 0f;

    protected virtual void Awake()
    {
        EnableRotation(true);
    }
    protected virtual void Update()
    {
        UpdateTargetDynamically();

        if (currentEnemy == null)
        {
            currentEnemy = FindEnemyWithinRange();
            return;
        }

        if (CanAttackEnemy())
        {
            AttackEnemy();
        }


        if (Vector3.Distance(transform.position, currentEnemy.GetCenterPoint()) > attackRange)
        {
            currentEnemy = null;
        }

        RotateTowerTowardsEnemy();
    }

    private void UpdateTargetDynamically()
    {
        if (dynamicTargetChange && Time.time >= lastTargetCheckTime + targetCheckInterval)
        {
            lastTargetCheckTime = Time.time;
            Enemy newEnemy = FindEnemyWithinRange();
            if (newEnemy != null && newEnemy != currentEnemy)
            {
                currentEnemy = newEnemy;
            }
        }
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
            Vector3 direction = GetDirectionToEnemyFrom(towerHead);
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    protected Vector3 GetDirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.GetCenterPoint() - startPoint.position).normalized;
    }

    protected Enemy FindEnemyWithinRange()
    {
        List<Enemy> priorityEnemies = new List<Enemy>();
        List<Enemy> enemiesInRange = new List<Enemy>();
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemyCollider in enemiesColliders)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            EnemyType enemyType = enemy != null ? enemy.GetEnemyType() : EnemyType.None;

            if (enemy != null)
            {
                if (enemyType == enemyPriorityType)
                {
                    priorityEnemies.Add(enemy);
                }
                else
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        if (priorityEnemies.Count > 0)
        {
            return GetClosestEnemy(priorityEnemies);
        }

        return GetClosestEnemy(enemiesInRange);
    }

    private static Enemy GetClosestEnemy(List<Enemy> enemiesInRange)
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

        return closestEnemy != null ? closestEnemy : null;
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
