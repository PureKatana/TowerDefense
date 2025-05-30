using UnityEngine;

public class Tower_Crossbow : Tower
{
    private Tower_Crossbow_Visuals visuals;

    [Header("Crossbow Details")]
    [Space]
    [SerializeField] private Transform gunPoint;
    
    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<Tower_Crossbow_Visuals>();
    }
    protected override void AttackEnemy()
    {
        Vector3 directionToEnemy = GetDirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hit, Mathf.Infinity, whatIsEnemy))
        {
            // Snappy effect
            towerHead.forward = directionToEnemy;

            visuals.TriggerLaserVisual(gunPoint.position, hit.point);
            visuals.TriggerReloadFX(attackCooldown); 

            IDamageable damageable = hit.transform.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }

}
