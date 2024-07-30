using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasCollided = false;
    private bool isStuck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!hasCollided)
        {
            // Rotate the arrow to align with its velocity direction
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        if (isStuck)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Player.Instance.OnArrowHit();

        // Check if the collided object does not have the tag "Player"
        if (!collision.gameObject.CompareTag("Player"))
        {
            // Stop the rotation when the arrow collides with any object other than the player
            hasCollided = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Stick the arrow to the enemy
            transform.SetParent(collision.transform); // Parent the arrow to the enemy
            isStuck = true;

        }


    }
}
