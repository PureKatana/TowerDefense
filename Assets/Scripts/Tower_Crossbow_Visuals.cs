using System.Collections;
using UnityEngine;

public class Tower_Crossbow_Visuals : MonoBehaviour
{
    private Tower_Crossbow towerCrossbow;

    [SerializeField] private LineRenderer laser;
    [SerializeField] private float laserDuration = 0.1f;

    private void Awake()
    {
        towerCrossbow = GetComponent<Tower_Crossbow>();
    }
    public void TriggerLaserVisual(Vector3 startPosition, Vector3 endPosition)
    {
        StartCoroutine(ShowLaserCoroutine(startPosition, endPosition));
    }

    private IEnumerator ShowLaserCoroutine(Vector3 startPosition, Vector3 endPosition)
    {
        towerCrossbow.EnableRotation(false);
        laser.enabled = true;

        laser.SetPosition(0, startPosition);
        laser.SetPosition(1, endPosition);

        yield return new WaitForSeconds(laserDuration);

        laser.enabled = false;
        towerCrossbow.EnableRotation(true);
    }
}
