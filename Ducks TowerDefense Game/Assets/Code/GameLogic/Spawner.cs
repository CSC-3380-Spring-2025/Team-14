using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour{
    public GameObject[] enemyPrefabs; // The enemy prefab to spawn
    public Transform spawnPoint; // The location where enemies will spawn
   
    public int baseEnemies = 2;

    private int waveNumber = 0; // Current wave number
    private int enemiesInCurrentWave = 0;

    private WaveTimer waveTimer; // Reference to the WaveTimer script

    void Start(){
        waveTimer = FindFirstObjectByType<WaveTimer>();
        if (waveTimer == null) Debug.LogError("WaveTimer not found!");
    }
    // Add similar wave completion logic for redundancy
    void Update()
    {
        if (waveTimer.IsWaveActive() && GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
            waveTimer.OnWaveDefeated();
        }
    }
    
// Start is called once before the first execution of Update after the MonoBehaviour is created
//--------------------------------------------------------------------
    public void StartWave(int wave){
        StopAllCoroutines(); // Stop any previous wave spawning
        waveNumber = wave; // Update the wave number
        enemiesInCurrentWave = CalculateEnemiesForWave(waveNumber);
        Enemy.enemiesRemaining = 0; // Reset the enemy counter
        Debug.Log($"Starting wave {waveNumber} with {enemiesInCurrentWave} enemies");
        StartCoroutine(SpawnWave()); // Start spawning
    }
//--------------------------------------------------------------------


    private int CalculateEnemiesForWave(int wave) => baseEnemies + Mathf.FloorToInt(wave  * (1 + wave * 0.1f));
    


// SpawnWave is called to spawn a wave of enemies
//Ienumerator is a coroutine, which is a function that can pause execution and return control to Unity but then continue where it left off on the following frame
//This is useful for creating animations, moving objects, and other tasks that require a series of steps to be completed over time
//--------------------------------------------------------------------
    IEnumerator SpawnWave(){
        Debug.Log("Spawning wave...");
        bool regenBoss = (waveNumber % 3 == 0) && waveNumber > 0; // Check if it's a regen boss wave
        bool resurBoss = (waveNumber % 6 == 0) && waveNumber > 0; // Check if it's a resurrection boss wave
        bool isEmperorBoss = (waveNumber % 9 == 0) && waveNumber > 0; // Check if it's an emperor boss wave

        // == SPAWN BOSS ==
        if(regenBoss && enemyPrefabs.Length > 3){
            SpawnEnemy(enemyPrefabs[3]); // Spawn boss
            yield return new WaitForSeconds(1f); // Extra delay for boss
            enemiesInCurrentWave--; // Boss counts as one enemy
        }
        if(resurBoss && enemyPrefabs.Length > 4){
            SpawnEnemy(enemyPrefabs[4]); // Spawn boss
            yield return new WaitForSeconds(1f); // Extra delay for boss
            enemiesInCurrentWave--; // Boss counts as one enemy
        }
        if(isEmperorBoss && enemyPrefabs.Length > 5){
            for (int i = 0; i < 5; i++){
                SpawnEnemy(enemyPrefabs[1]); // Spawn boss minions
                yield return new WaitForSeconds(0.5f); // Extra delay for boss minions
            }
            SpawnEnemy(enemyPrefabs[5]); // Spawn boss
            yield return new WaitForSeconds(1f); // Extra delay for boss
            enemiesInCurrentWave--; // Boss counts as one enemy
        }
        // == END SPAWN BOSS ==

        // Spawn regular enemies
        for (int i = 0; i < enemiesInCurrentWave; i++){
            int randomEnemy = Random.Range(0, 3); // Only regular enemies (0-2)
            SpawnEnemy(enemyPrefabs[randomEnemy]);
            
            // Variable spawn delay that decreases as wave progresses
            float delay = Mathf.Max(0.1f, 0.5f - (waveNumber * 0.02f));
            yield return new WaitForSeconds(delay);
        }
    }
//--------------------------------------------------------------------





// SpawnEnemy is called to spawn a single enemy
//--------------------------------------------------------------------
    void SpawnEnemy(GameObject enemyPrefab){
        if(enemyPrefab == null) return; // Check if the enemy prefab is assigned

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Enemy.enemiesRemaining++; // Increment enemy counter
    }
//--------------------------------------------------------------------


    public void ResetSpawner(){
        Debug.Log("Spawner has been reset.");
        StopAllCoroutines(); // Add this
        waveNumber = 0; // Reset wave number
        enemiesInCurrentWave = 0; // Reset enemies in current wave
        Enemy.enemiesRemaining = 0; // Reset the enemy counter
    }
}//end of class