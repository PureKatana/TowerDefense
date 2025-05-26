using System.Collections;
using UnityEngine;

public class Tower_Crossbow_Visuals : MonoBehaviour
{
    private Tower_Crossbow towerCrossbow;

    [Header("Crossbow Visuals")]
    [Space]
    [SerializeField] private LineRenderer laser;
    [SerializeField] private float laserDuration = 0.1f;

    [Header("Glowing Visuals")]
    [Space]
    [SerializeField] private MeshRenderer crossbowMeshRenderer;
    private Material glowingMaterial;
    [Space]
    [SerializeField] private float maxIntensity = 150.0f;
    private float currentIntensity;
    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private void Awake()
    {
        towerCrossbow = GetComponent<Tower_Crossbow>();
        glowingMaterial = new Material(crossbowMeshRenderer.material); // Create a new instance to avoid modifying the original material

        crossbowMeshRenderer.material = glowingMaterial; // Assign the new material to the mesh renderer

        StartCoroutine(ChangeEmission(1.0f)); // Initialize the emission color
    }

    private void Update()
    {
        UpdateEmissionColor();
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);
        emissionColor *= Mathf.LinearToGammaSpace(currentIntensity); // Scale the color by the current intensity
        glowingMaterial.SetColor("_EmissionColor", emissionColor);
    }

    public void TriggerReloadFX(float duration)
    {
        StartCoroutine(ChangeEmission(duration / 2));
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

    private IEnumerator ChangeEmission(float duration)
    {
        float startTime = Time.time;

        float startIntensity = 0.0f;

        while (Time.time < startTime + duration)
        {
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, (Time.time - startTime) / duration);
            yield return null;
        }

        currentIntensity = maxIntensity; // Ensure we end at the max intensity
    }
}
