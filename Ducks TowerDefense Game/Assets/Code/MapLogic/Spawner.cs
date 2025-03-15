using UnityEngine;
using System.Collections;
using TMPro;

public class Spawner : MonoBehaviour
{
    public Transform enemy; // The enemy prefab to spawn
    public Transform spawnPoint; // The location where enemies will spawn

    public TextMeshProUGUI CountDownTimerText; // UI Text for countdown
    private int waveNumber = 0; // Current wave number

    // Start a new wave (called by WaveTimer)
    public void StartWave(int wave)
    {
        waveNumber = wave; // Update the wave number
        StartCoroutine(SpawnWave()); // Start spawning
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < waveNumber; i++) // Spawn more enemies as waves increase
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}