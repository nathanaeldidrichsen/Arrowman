using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Parameters for camera movement
    [SerializeField] private float moveSpeed = 5f;      // Speed at which the camera moves
    [SerializeField] private float minX = -10f;         // Minimum x position the camera can move to
    [SerializeField] private float maxX = 10f;          // Maximum x position the camera can move to
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing factor for the camera movement

    private Vector3 targetPosition;                     // Target position for the camera movement

    private void Start()
    {
        // Initialize the target position to the current position of the camera
        targetPosition = transform.position;
    }

    private void Update()
    {
        // Get horizontal input from the user
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the target position based on the horizontal input
        float targetX = Mathf.Clamp(transform.position.x + horizontalInput * moveSpeed * Time.deltaTime, minX, maxX);

        // Update the target position
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
