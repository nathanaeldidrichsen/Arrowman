using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    private int health = 100;
    public float moveSpeed = 2f;
    private float canMoveTime = 1f; // Tracks how long the enemy can move
    private float canMoveCounter = 0;
    private bool isAttacking = false; // To track if currently attacking
    private float attackCooldown = 2f; // Cooldown time between attacks
    private float lastAttackTime = 0f; // Track the last time the enemy attacked

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    // Method to move the enemy
    private void Move()
    {
        if (canMoveCounter < canMoveTime)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            canMoveCounter += Time.deltaTime;
        }
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle enemy death
    private void Die()
    {
        OnDeath?.Invoke(); // Trigger the OnDeath event
        Destroy(gameObject); // Destroy the enemy game object
    }

    // Method to handle collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Castle"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                InvokeRepeating(nameof(AttackCastle), 0f, attackCooldown);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Castle"))
        {
            isAttacking = false;
            CancelInvoke(nameof(AttackCastle));
        }
    }

    // Method to attack the castle
    private void AttackCastle()
    {
        if (isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            // Add attack logic here (e.g., reduce castle's health)
            Debug.Log("Attacking the castle!");
            lastAttackTime = Time.time;
        }
    }

    // Static method to notify enemies to move
    public static void NotifyEnemiesToMove()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.canMoveCounter = 0f; // Reset the movement time for all enemies
        }
    }
}
