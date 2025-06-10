using System.Collections;
using UnityEngine;

public class Tower_Crossbow_Visuals : MonoBehaviour
{
    private Tower_Crossbow towerCrossbow;
    private Enemy enemy;

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

    [Header("Rotor Visuals")]
    [Space]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorLoaded;
    [SerializeField] private Transform rotorUnloaded;

    [Header("Front String Visuals")]
    [Space]
    [SerializeField] private LineRenderer frontStringRenderer_L;
    [SerializeField] private LineRenderer frontStringRenderer_R;
    [Space]
    [SerializeField] private Transform frontStringStartPoint_L;
    [SerializeField] private Transform frontStringEndPoint_L;
    [SerializeField] private Transform frontStringStartPoint_R;
    [SerializeField] private Transform frontStringEndPoint_R;

    [Header("Back String Visuals")]
    [Space]
    [SerializeField] private LineRenderer backStringRenderer_L;
    [SerializeField] private LineRenderer backStringRenderer_R;
    [Space]
    [SerializeField] private Transform backStringStartPoint_L;
    [SerializeField] private Transform backStringEndPoint_L;
    [SerializeField] private Transform backStringStartPoint_R;
    [SerializeField] private Transform backStringEndPoint_R;

    [SerializeField] private LineRenderer[] lineRenderers;


    private void Awake()
    {
        towerCrossbow = GetComponent<Tower_Crossbow>();
        glowingMaterial = new Material(crossbowMeshRenderer.material); // Create a new instance to avoid modifying the original material
        crossbowMeshRenderer.material = glowingMaterial; // Assign the new material to the mesh renderer

        AssignGlowingMaterialToLines();

        StartCoroutine(ChangeEmission(1.0f)); // Initialize the emission color

        rotor.position = rotorLoaded.position; // Start with the rotor in the loaded position
    }

    private void AssignGlowingMaterialToLines()
    {
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.material = glowingMaterial; // Assign the glowing material to all line renderers
        }
    }

    private void Update()
    {
        UpdateEmissionColor();
        UpdateStrings();

        if (laser.enabled && enemy != null)
        {
            // Update the laser position to follow the current enemy
            laser.SetPosition(1, enemy.GetCenterPoint());
        }
    }

    private void UpdateStrings()
    {
        UpdateStringsVisuals(frontStringRenderer_L, frontStringStartPoint_L, frontStringEndPoint_L);
        UpdateStringsVisuals(frontStringRenderer_R, frontStringStartPoint_R, frontStringEndPoint_R);
        UpdateStringsVisuals(backStringRenderer_L, backStringStartPoint_L, backStringEndPoint_L);
        UpdateStringsVisuals(backStringRenderer_R, backStringStartPoint_R, backStringEndPoint_R);
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);
        emissionColor *= Mathf.LinearToGammaSpace(currentIntensity); // Scale the color by the current intensity
        glowingMaterial.SetColor("_EmissionColor", emissionColor);
    }

    public void TriggerReloadFX(float duration)
    {
        float newDuration = duration / 2.0f;

        StartCoroutine(ChangeEmission(newDuration));
        StartCoroutine(UpdateRotorPosition(newDuration));
    }

    public void TriggerLaserVisual(Vector3 startPosition, Vector3 endPosition)
    {
        StartCoroutine(ShowLaserCoroutine(startPosition, endPosition));
    }

    private IEnumerator ShowLaserCoroutine(Vector3 startPosition, Vector3 endPosition)
    {
        enemy = towerCrossbow.currentEnemy; // Get the current enemy for the laser effect
        laser.enabled = true;

        laser.SetPosition(0, startPosition);
        laser.SetPosition(1, endPosition);

        yield return new WaitForSeconds(laserDuration);

        laser.enabled = false;
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

    private IEnumerator UpdateRotorPosition(float duration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, t);
            yield return null;
        }

        rotor.position = rotorLoaded.position; // Ensure we end at the loaded position
    }

    private void UpdateStringsVisuals(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
