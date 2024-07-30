using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName; // Name of the wave
    public List<GameObject> enemyPrefabs; // Prefabs of enemies to spawn
    public int numberOfEnemies; // Number of enemies to spawn
    public float spawnInterval; // Time interval between spawning enemies
}
