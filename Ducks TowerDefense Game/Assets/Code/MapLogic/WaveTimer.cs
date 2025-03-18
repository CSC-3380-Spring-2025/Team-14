using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveTimer : MonoBehaviour
{
    public Text waveText;  // UI Text to show wave info
    public float timeBetweenWaves = 5f; // Time before the next wave
    private int currentWave = 0;
    private float countDown;

    private Spawner spawner; // Reference to the Spawner script

    void Start()
    {
        
        UpdateWaveText();
        StartCoroutine(WaveLoop());
        countDown = timeBetweenWaves;

        // Find and link the Spawner script in the scene
        spawner = Object.FindAnyObjectByType<Spawner>();
    }

    void Update()
    {
        countDown -= Time.deltaTime;
        UpdateWaveText();
    }

    IEnumerator WaveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves); // Wait for the countdown to finish

            StartNewWave();
        }
    }

    void StartNewWave()
    {
        currentWave++; // Increase wave count
        countDown = timeBetweenWaves; // Reset countdown

        if (spawner != null)
        {
            spawner.StartWave(currentWave); // Tell the spawner to start a new wave
        }

        UpdateWaveText();
    }

    void UpdateWaveText()
    {
        waveText.text = $"Wave: {currentWave} Next: {Mathf.Ceil(countDown)}s";
    }
}