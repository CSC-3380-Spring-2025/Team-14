using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour{
    public GameObject[] enemyPrefabs; // The enemy prefab to spawn
    public Transform spawnPoint; // The location where enemies will spawn
    private int waveNumber = 0; // Current wave number
    private int addPreviousEnemy = 0; // Number of enemies remaining in the current wave

    


    
// Start is called once before the first execution of Update after the MonoBehaviour is created
//--------------------------------------------------------------------
    public void StartWave(int wave){
        Debug.Log($"Starting wave {wave}");
        waveNumber = wave; // Update the wave number
        Enemy.enemiesRemaining = 0; // Reset the enemy counter
        StartCoroutine(SpawnWave()); // Start spawning
    }
//--------------------------------------------------------------------





// SpawnWave is called to spawn a wave of enemies
//Ienumerator is a coroutine, which is a function that can pause execution and return control to Unity but then continue where it left off on the following frame
//This is useful for creating animations, moving objects, and other tasks that require a series of steps to be completed over time
//--------------------------------------------------------------------
    IEnumerator SpawnWave(){
        Debug.Log("Spawning wave...");
        // Spawn enemies in the wave
        int addEnemy = Random.Range(0, 3);
        addPreviousEnemy = addEnemy + waveNumber + addPreviousEnemy; // Calculate the number of enemies to spawn
        for (int i = 0; i < addPreviousEnemy; i++){
            Debug.Log($"Spawning enemy {i + 1} of {addPreviousEnemy}");
            SpawnEnemy();
            yield return new WaitForSeconds(0.3f);
        }
    }
//--------------------------------------------------------------------





// SpawnEnemy is called to spawn a single enemy
//--------------------------------------------------------------------
    void SpawnEnemy(){
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation); // Spawn the enemy at the spawn point
    }
//--------------------------------------------------------------------
}//end of class