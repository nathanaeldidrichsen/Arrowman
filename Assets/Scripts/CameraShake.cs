using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Shake parameters
    [SerializeField] private float shakeDuration = 0.5f; // Total duration of the shake
    [SerializeField] private float shakeIntensity = 0.1f; // Intensity of the shake
    [SerializeField] private float dampingSpeed = 1.0f; // Speed at which the shake damps out

    private Vector3 originalPosition; // To store the original position of the camera
    private Coroutine shakeCoroutine; // Reference to the current shake coroutine

    private void Start()
    {
        // Store the original position
        originalPosition = transform.localPosition;
    }

    // Method to trigger the camera shake
    public void ScreenShake(float duration = -1, float intensity = -1)
    {
        originalPosition= transform.localPosition;

        // If a shake is already running, stop it
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        // Use default values if parameters are not provided
        duration = duration < 0 ? shakeDuration : duration;
        intensity = intensity < 0 ? shakeIntensity : intensity;

        // Start a new shake coroutine
        shakeCoroutine = StartCoroutine(Shake(duration, intensity));
    }

    // Coroutine to handle the shake effect
    private IEnumerator Shake(float duration, float intensity)
    {
        float elapsed = 0.0f; // Track the elapsed time

        while (elapsed < duration)
        {
            // Create a random offset for the shake
            float offsetX = Random.Range(-1f, 1f) * intensity;
            float offsetY = Random.Range(-1f, 1f) * intensity;

            // Apply the offset to the camera's position
            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            // Increment the elapsed time
            elapsed += Time.deltaTime;

            // Dampen the intensity over time
            intensity = Mathf.Lerp(intensity, 0f, Time.deltaTime * dampingSpeed);

            // Wait for the next frame
            yield return null;
        }

        // Reset the camera to its original position
        transform.localPosition = originalPosition;

        // Clear the shake coroutine reference
        shakeCoroutine = null;
    }
}
