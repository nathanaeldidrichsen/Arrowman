using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Transform firePoint; // The point from where the arrow will be fired
    public GameObject arrowPrefab; // The arrow prefab
    public float maxArrowSpeed = 20f; // Maximum speed of the arrow
    public int damage = 1;
    public TextMeshProUGUI speedText; // Reference to the TextMeshProUGUI element
    private GameManager gameManager;
    private SoundManager soundManager;


    private Vector3 initialClickPosition;
    private bool isDragging = false;
    public event Action<int> OnDamageChanged;

    private bool hasPlayedError = false; // Flag to track if the error state has been triggered
    private bool hasPlayedDrawSound = false; // Flag to track if the draw sound has been played


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

    void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
    }


    void Update()
    {
        // Check if the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Ignore input if the pointer is over a UI element
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Record the initial position where the mouse is clicked
            initialClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialClickPosition.z = 0;
            isDragging = true;

            // Reset the error flag if there are arrows available
            hasPlayedError = false;
            hasPlayedDrawSound = false; // Reset the draw sound flag
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            // Update the speed text while dragging
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;

            if (gameManager.Arrows > 0)
            {
                float distance = Vector3.Distance(initialClickPosition, currentMousePosition);
                float speed = Mathf.Min(2 * distance, maxArrowSpeed);
                speedText.text = "Drawforce: " + speed.ToString("F2");
                UpdateSpeedTextPosition(currentMousePosition);

                if (!hasPlayedDrawSound)
                {
                    soundManager.PlayArrowDraw();
                    hasPlayedDrawSound = true; // Set the flag to true to ensure the draw sound plays only once
                }
            }
            else
            {
                UpdateSpeedTextPosition(currentMousePosition);
                if (!hasPlayedError)
                {
                    soundManager.PlayError();
                    hasPlayedError = true; // Set the flag to true to prevent repeated error state
                }
                speedText.text = "Out of arrows!";
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            speedText.text = "";
            if (gameManager.Arrows <= 0)
            {
                isDragging = false;
                return;
            }
            // Record the release position and fire the arrow
            Vector3 releasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            releasePosition.z = 0;
            float distance = Vector3.Distance(initialClickPosition, releasePosition);
            float speed = Mathf.Min(3 * distance, maxArrowSpeed);
            FireArrow(initialClickPosition, speed);
            gameManager.Arrows--;

            soundManager.PlayArrowShoot();
            soundManager.PlayArrowRelease();

            isDragging = false;
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
        arrow.GetComponent<Arrow>().arrowDamage = damage;

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


    public int Damage
    {
        get => damage;
        set
        {
            if (damage != value)
            {
                damage = value;
                OnDamageChanged?.Invoke(damage);  // Trigger the event
            }
        }
    }
}
