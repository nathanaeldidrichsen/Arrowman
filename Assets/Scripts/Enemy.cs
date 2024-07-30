using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    public int health = 3;
    public int damage = 1;
    public float moveSpeed = 2f;
    public int goldDrop = 1;
    private float canMoveTime = 1f; // Tracks how long the enemy can move
    private float canMoveCounter = 0;
    private bool isAttacking = false; // To track if currently attacking
    private float attackCooldown = 2f; // Cooldown time between attacks
    private float lastAttackTime = 0f; // Track the last time the enemy attacked
    [SerializeField] private GameObject hurtParticle;
    [SerializeField] private float distanceToCastle;
        private float randomAttackDelay = 0f; // Random delay before starting the first attack
    private float randomCooldownVariation = 0f; // Random variation in attack cooldown

    public float stoppingRange = 1f; // Range within which the enemy starts attacking

    public bool canStartMoving = false; // Flag to start moving
    private bool limitedMovementActive = false; // Flag to start limited movement counter
    private bool isWithinRange;

    void Start()
    {
                // Introduce a small random delay and cooldown variation for desynchronizing attacks
        randomAttackDelay = UnityEngine.Random.Range(0f, 0.5f);
        randomCooldownVariation = UnityEngine.Random.Range(-0.2f, 0.2f);
    }

    private void Update()
    {

        if (Castle.Instance != null)
        {
            distanceToCastle = Vector2.Distance(transform.position, Castle.Instance.transform.position);
        }

        if (canStartMoving)
        {
            // Only move if not attacking and not within stopping range
            if (!isAttacking && distanceToCastle > stoppingRange)
            {
                Move();
            }

            // Check and attack the castle if within range
            CheckAndAttackCastle();
        }
    }

    private void Move()
    {
        // Continue moving indefinitely until limited movement starts
        if (limitedMovementActive)
        {
            if (canMoveCounter < canMoveTime)
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                canMoveCounter += Time.deltaTime;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }
    private void CheckAndAttackCastle()
    {
        // Check if the enemy is within the stopping range of the castle
        if (distanceToCastle <= stoppingRange)
        {
            // Begin attack if within stopping range and not currently attacking
            if (!isAttacking)
            {
                isAttacking = true;
                Invoke(nameof(PerformAttack), randomAttackDelay); // Start attack with a delay
            }
        }
    }

    private void PerformAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown + randomCooldownVariation)
        {
            AttackCastle();
            lastAttackTime = Time.time;

            // Stop moving after performing one attack
            canStartMoving = false;
            isAttacking = false;
        }
    }

    private void AttackCastle()
    {
        // Attack the castle
        Castle.Instance.TakeDamage(damage);
        GameManager.Instance.Gold++;
        Debug.Log("Attacking the castle!");

        // Reset attack delay for next attack cycle
        randomAttackDelay = UnityEngine.Random.Range(0f, 0.5f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        Instantiate(hurtParticle, transform.position, Quaternion.identity);
    }

    private void Die()
    {
        OnDeath?.Invoke(); // Trigger the OnDeath event
        Invoke(nameof(DeathLogic), 0.9f); // Play sound after 0.9 seconds
        Destroy(gameObject, 1); // Destroy the enemy game object
    }

    private void DeathLogic()
    {
        GameManager.Instance.AddGold(goldDrop);
    }

    public void StartMoving()
    {
        canStartMoving = true;
    }

    public void StartLimitedMovement()
    {
        limitedMovementActive = true;
        canMoveCounter = 0f; // Reset the movement time counter

        // Allow another attack if within range after movement
        isAttacking = false;
        canStartMoving = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Lava"))
        {
            TakeDamage(30);
        }
    }

    public static void NotifyEnemiesToMove()
    {
        WavesManager manager = FindObjectOfType<WavesManager>();
        if (manager != null)
        {
            manager.NotifyWaveToMove();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingRange);
    }
}