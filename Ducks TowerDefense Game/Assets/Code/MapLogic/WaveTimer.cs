using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveTimer : MonoBehaviour
{
    public Text waveText;  // UI Text to show wave info
    public float timeBetweenWaves = 10f; // Time before the next wave
    private int currentWave = 0;
    private float countdown;

    private Spawner spawner; // Reference to the Spawner script

    void Start()
    {
        countdown = timeBetweenWaves;
        UpdateWaveText();
        StartCoroutine(WaveLoop());

        // Find and link the Spawner script in the scene
        spawner = FindObjectOfType<Spawner>();
    }

    void Update()
    {
        countdown -= Time.deltaTime;
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
        countdown = timeBetweenWaves; // Reset countdown

        if (spawner != null)
        {
            spawner.StartWave(currentWave); // Tell the spawner to start a new wave
        }

        UpdateWaveText();
    }

    void UpdateWaveText()
    {
        waveText.text = $"Wave: {currentWave} Next: {Mathf.Ceil(countdown)}s";
    }
}