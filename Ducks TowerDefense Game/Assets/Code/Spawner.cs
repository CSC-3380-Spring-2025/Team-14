using UnityEngine;
using System.Collections;
using TMPro;
public class Spawner : MonoBehaviour{

    public Transform enemy; // The enemy prefab to spawn
    public Transform spawnPoint; // The location where enemies will spawn


    public float waveTime = 5f; // Time between waves
    private float countDown = 2f; // Initial delay before the first wave
    public TextMeshProUGUI CountDownTimerText; // Reference to the UI Text element for the countdown timer
    private int waveNumber = 0;// Current wave number

    void Update(){
        // Count down to the next wave
        if(countDown <= 0f){
            // Start spawning a wave
            StartCoroutine(SpawnWave()); //Because SpawnWave method is a IEnumerator, you will have to code StartCoroutine and have SpawnWave() as its parameter
            countDown = waveTime;// Reset the countdown
        }

        countDown -= Time.deltaTime;


        // Update the countdown timer text
        if (CountDownTimerText != null){
            CountDownTimerText.text = Mathf.Round(countDown).ToString();
        }
        
    }

    //IEnumerator orginally a void, when we had the void, the enemy overlap each other
    //But having IEnumerator you can call "yield return new WaitForSeconds(0.5f); this pauses the loop for 0.5 sec each time cauing the enemy to not overlap
    // Coroutine to spawn a wave of enemies
    IEnumerator SpawnWave(){ 

        waveNumber++; // Increase the wave number

        for(int i = 0; i < waveNumber; i++){ //Every time we spawn a wave, wavenumber increase
            SpawnEnemy(); // Spawn a single enemy
            yield return new WaitForSeconds(0.5f);// Wait 0.5 seconds before spawning the next enemy
        }

    }

    // Method to spawn a single enemy
    void SpawnEnemy(){
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation); // Spawn the enemy at the spawn point
    }



}
