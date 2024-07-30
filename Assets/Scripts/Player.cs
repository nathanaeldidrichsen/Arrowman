using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Transform firePoint; // The point from where the arrow will be fired
    public GameObject arrowPrefab; // The arrow prefab
    public float maxArrowSpeed = 20f; // Maximum speed of the arrow
    public TextMeshProUGUI speedText; // Reference to the TextMeshProUGUI element

    private Vector3 initialClickPosition;
    private bool isDragging = false;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of Player exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Record the initial position where the mouse is clicked
            initialClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialClickPosition.z = 0;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            // Update the speed text while dragging
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;
            float distance = Vector3.Distance(initialClickPosition, currentMousePosition);
            float speed = Mathf.Min(2*distance, maxArrowSpeed);
            speedText.text = "Drawforce: " + speed.ToString("F2");

            // Update the position of the speed text
            UpdateSpeedTextPosition(currentMousePosition);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            // Record the release position and fire the arrow
            Vector3 releasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            releasePosition.z = 0;
            float distance = Vector3.Distance(initialClickPosition, releasePosition);
            float speed = Mathf.Min(3*distance, maxArrowSpeed);
            FireArrow(initialClickPosition, speed);
            isDragging = false;
            speedText.text = ""; // Clear the speed text when not dragging
        }
    }


    // Call this method when an arrow hits something
    public void OnArrowHit()
    {
        Enemy.NotifyEnemiesToMove(); // Notify enemies to move
    }

    void FireArrow(Vector3 targetPosition, float speed)
    {
        // Calculate direction based on the initial click position
        Vector3 direction = (targetPosition - firePoint.position).normalized;

        // Instantiate the arrow
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        // Rotate the arrow to point towards the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Apply velocity to the arrow's Rigidbody2D
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    void UpdateSpeedTextPosition(Vector3 mousePosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(mousePosition);
        speedText.transform.position = new Vector3(screenPosition.x, screenPosition.y + 40, screenPosition.z);
    }
}