using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool GameEnded = false; 
    private bool GameWin = false; // Flag to check if the win UI is active
    public GameObject GameOverUI; // Reference to the Game Over UI
    public GameObject GameWinUI; // Reference to the Win UI

    void Start()
    {
        // Ensure that UI elements are assigned when the game starts or after a scene reload
        if (GameOverUI == null) GameOverUI = GameObject.Find("GameOverUI");
        if (GameWinUI == null) GameWinUI = GameObject.Find("GameWinUI"); 
    
        GameEnded = false; // Initialize GameEnded to false
        GameWin = false; // Initialize GameWin to false
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEnded) return; // If the game is ended, do not process further
        if (GameWin) return; // If the game is won, do not process further

        if (PlayerStats.Lives <= 0) EndGame(); // Call EndGame if lives are 0 or less
        if (Input.GetKeyDown("e")) EndGame(); // Call EndGame when "E" is pressed
        if (Input.GetKeyDown("w")) WinGame(); // Call WinGame when "W" is pressed
    }

    void EndGame()
    {
        GameEnded = true;
        if(GameOverUI != null){
            GameOverUI.SetActive(true); // Show the Game Over UI
        // Ensure the Game Over UI animation uses unscaled time
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

    public void WinGame()
    {
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

    // Method to reset the game state
    public void ContinueGameState()
    {
        Debug.Log("Continue game state...");
        GameEnded = false; // Reset the GameEnded flag
        GameWin = false; // Reset the GameWin flag
        if (GameOverUI != null) GameOverUI.SetActive(false); // Hide the Game Over UI
        if (GameWinUI != null) GameWinUI.SetActive(false); // Hide the Win UI
        
        // Reset Player Stats if necessary
        PlayerStats.Rounds = 0; // Example of resetting rounds (if needed)

        // Resume game time
        Time.timeScale = 1; // Unpause the game
    }
}
