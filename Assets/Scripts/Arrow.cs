using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasCollided = false;
    private bool isStuck;
    public int arrowDamage;
    private int collisionCounter = 0; // to count how many times the arrow has collided

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
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
        // // Check if the collided object does not have the tag "Player"
        if (collision.gameObject.CompareTag("Ground") && !hasCollided)
        {
            SoundManager.Instance.PlayArrowHitGround();
            // collisionCounter++;
            // Stop the rotation when the arrow collides with any object other than the player
        }

        if (collision.gameObject.CompareTag("Castle") && !hasCollided)
        {

            SoundManager.Instance.PlayArrowHitWall();
            // Stop the rotation when the arrow collides with any object other than the player
        }

        if (collision.gameObject.CompareTag("Enemy") && !hasCollided)
        {
            GameManager.Instance.Arrows++;
            collision.gameObject.GetComponent<Enemy>().TakeDamage(arrowDamage);
            SoundManager.Instance.PlayArrowHitGround();
            SoundManager.Instance.PlayArrowHitEnemy();
            // Stick the arrow to the enemy
            transform.SetParent(collision.transform); // Parent the arrow to the enemy
            isStuck = true;

        }
        if (!hasCollided)
        {
            Player.Instance.OnArrowHit();
        }

        hasCollided = true;

    }
}
