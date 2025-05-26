using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        float newRotationSpeed = rotationSpeed * 100.0f;
        transform.Rotate(rotationVector * newRotationSpeed * Time.deltaTime);
    }
}
