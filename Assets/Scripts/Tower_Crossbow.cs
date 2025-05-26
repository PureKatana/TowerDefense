using UnityEngine;

public class Tower_Crossbow : Tower
{
    [Header("Crossbow Details")]
    [Space]
    [SerializeField] private Transform gunPoint;
    
    protected override void AttackEnemy()
    {
        Vector3 directionToEnemy = GetDirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hit, Mathf.Infinity, whatIsEnemy))
        {
            // Snappy effect
            towerHead.forward = directionToEnemy;

            Debug.Log(hit.collider.gameObject.name + " hit at " + hit.point);
            Debug.DrawLine(gunPoint.position, hit.point);
        }
    }

}
