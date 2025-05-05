using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool GameEnded = false; 
    private bool GameWin = false; // Flag to check if the win UI is active
    public GameObject GameOverUI; // Reference to the Game Over UI
    public GameObject GameWinUI; // Reference to the Win UI
    private WaveTimer waveTimer; // Reference to the WaveSpawner script
    private Spawner spawner; // Reference to the Spawner script
    private UnityEngine.UI.Text moneyText;
    private UnityEngine.UI.Text livesText;
    private UnityEngine.UI.Text roundsText;
//--------------------------------------------------------------------
    private void Start()
    {
        if (spawner == null) spawner = FindFirstObjectByType<Spawner>(); // Find the Spawner script in the scene
        if (waveTimer == null) waveTimer = FindFirstObjectByType<WaveTimer>(); // Find the WaveTimer script in the scene
        // Cache references if not assigned in the Inspector
        if (moneyText == null) moneyText = GameObject.Find("MoneyText")?.GetComponent<UnityEngine.UI.Text>();
        if (livesText == null) livesText = GameObject.Find("LivesText")?.GetComponent<UnityEngine.UI.Text>();
        if (roundsText == null) roundsText = GameObject.Find("WaveText")?.GetComponent<UnityEngine.UI.Text>();
    }
//--------------------------------------------------------------------
    void Update()
    {
        if (GameEnded) return; // If the game is ended, do not process further
        if (GameWin) return; // If the game is won, do not process further

        if (PlayerStats.Lives <= 0) EndGame(); // Call EndGame if lives are 0 or less
        if (Input.GetKeyDown("e")) EndGame(); // Call EndGame when "E" is pressed
        if (Input.GetKeyDown("w")) WinGame(); // Call WinGame when "W" is pressed
    }
//--------------------------------------------------------------------
    public void ResetGame(bool isFullReset = true){
        // 2. Unpause
        Time.timeScale = 1f; // Reset time scale in case game was paused

        // 1. Stop coroutines FIRST
        StopAllCoroutines(); // Stop any running coroutines

        
        // 3. Reset flags
        GameEnded = false;
        GameWin = false;
        

        // 4. Reset player stats
        PlayerStats.ResetAll(); 

        // 5. Clean up PlaceTurret singleton
        if (isFullReset && PlaceTurret.instance != null){
            Destroy(PlaceTurret.instance.gameObject);
            PlaceTurret.instance = null;
        }

        // 6. Reset other state
        // Clear all enemies
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) Destroy(enemy);

        // Clear all towers
        foreach(GameObject duck in GameObject.FindGameObjectsWithTag("Duck")) Destroy(duck);
        

        // Reset wave spawner
        if (waveTimer != null){
            waveTimer.ResetWaves();
            if (waveTimer.continueButton != null){
                waveTimer.continueButton.gameObject.SetActive(true);
                waveTimer.continueButton.interactable = true;
            }
            waveTimer.isWaveActive = false;
        }

        if (spawner != null) spawner.ResetSpawner(); // Reset wave count
        
        if (GameOverUI != null) GameOverUI.SetActive(false); // Hide the Game Over UI
        if (GameWinUI != null) GameWinUI.SetActive(false); // Hide the Win UI

        // Reset UI
        UpdateUI(); // Ensure the UI is updated after resetting the game
    }
//--------------------------------------------------------------------
    void EndGame()
    {
        if(GameEnded) return; // If the game is already ended, do not process further
        GameEnded = true;

        // hide all enemy visuals
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(enemy); 
        }

        if(GameOverUI != null){
            GameOverUI.SetActive(true); // Show the Game Over UI
        // Ensure the Game Over UI animation uses unscaled time
        GameOverUI.SetActive(true);
        GameOverUI.transform.SetAsLastSibling();
        Animator animator = GameOverUI.GetComponent<Animator>();
        if (animator != null) animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        } 
        else {
            Debug.LogError("GameOverUI is not assigned or found in the scene.");
        }
        

        Debug.Log("Game Over!");

        // Pause the game
        Time.timeScale = 0;
    }
//--------------------------------------------------------------------
    public void WinGame()
    {
        if(GameWin) return; // If the game is already won, do not process further
        GameWin = true;

        if(GameWinUI != null){
            GameWinUI.SetActive(true); // Show the Win UI
            // Ensure the Win UI animation uses unscaled time
            Animator animator = GameWinUI.GetComponent<Animator>();
            if (animator != null) animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else {
            Debug.LogError("GameWinUI is not assigned or found in the scene.");
        }
        Debug.Log("You Win!");

        // Pause the game
        Time.timeScale = 0;

        PlayerPrefs.SetInt("LevelProgress", PlayerPrefs.GetInt("LevelProgress") + 1); // Increment the level reached
        PlayerPrefs.Save(); // Save the updated level reached
    }
//--------------------------------------------------------------------
    // Continue the game state after a win
    public void ContinueGameState()
    {
        Debug.Log("Continue game state...");
        GameEnded = false; // Reset the GameEnded flag
        GameWin = false; // Reset the GameWin flag
        if (GameOverUI != null) GameOverUI.SetActive(false); // Hide the Game Over UI
        if (GameWinUI != null) GameWinUI.SetActive(false); // Hide the Win UI
        
        // Resume game time
        Time.timeScale = 1; // Unpause the game
    }
//--------------------------------------------------------------------
    // Method to update the UI elements (e.g., money, lives, rounds)
    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = "Money: " + PlayerStats.Money.ToString();
        else Debug.LogError("MoneyText UI element not found.");
    }
//--------------------------------------------------------------------
}
