using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public List<Wave> waves; // List of waves to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float timeBetweenWaves = 20f; // Time between waves
    public float waveStartDelay = 3f; // Delay before starting the first wave

    private int currentWaveIndex = 0;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;
    private bool waveFullySpawned = false; // New flag to track if all enemies have spawned

    private bool waveInProgress = false;
    private List<Enemy> currentWaveEnemies = new List<Enemy>(); // Track enemies for current wave


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartWaveAfterDelay(waveStartDelay)); // Start the first wave after a delay
    }

    // Method to start a wave
    private void StartWave()
    {
        if (currentWaveIndex < waves.Count)
        {
            waveInProgress = true;
            waveFullySpawned = false; // Reset the flag for the new wave
            Wave currentWave = waves[currentWaveIndex];
            StartCoroutine(SpawnWave(currentWave));
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    // Coroutine to spawn a wave
    private IEnumerator SpawnWave(Wave wave)
    {
        enemiesSpawned = 0;
        enemiesAlive = wave.numberOfEnemies;
        currentWaveEnemies.Clear(); // Clear the list for the current wave

        while (enemiesSpawned < wave.numberOfEnemies)
        {
            SpawnEnemy(wave);
            enemiesSpawned++;
            yield return new WaitForSeconds(wave.spawnInterval); // Wait before spawning next enemy
        }

        waveFullySpawned = true; // Mark the wave as fully spawned
    }

    // Method to spawn an enemy
    private void SpawnEnemy(Wave wave)
    {
        SoundManager.Instance.PlaySpawnWave();
        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Choose a random enemy prefab
        GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];

        // Instantiate enemy at the chosen spawn point
        GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = enemyGO.GetComponent<Enemy>();

        // Add the enemy to the current wave list
        currentWaveEnemies.Add(enemy);

        // Subscribe to the enemy's death event
        enemy.OnDeath += OnEnemyDeath;

        // Start initial move so they don't stack
        enemy.StartMoving();
    }

    // Event handler for when an enemy dies
    private void OnEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && waveInProgress)
        {
            waveInProgress = false;
            currentWaveIndex++;
            StartCoroutine(StartWaveAfterDelay(timeBetweenWaves));
        }
    }

    // Notify the wave to move when arrow hits
    public void NotifyWaveToMove()
    {
        if (!waveFullySpawned) return; // Only allow movement if the wave is fully spawned

        foreach (Enemy enemy in currentWaveEnemies)
        {
            enemy.StartLimitedMovement();
        }
    }

    // Coroutine to start the next wave after a delay
    private IEnumerator StartWaveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartWave();
    }
}