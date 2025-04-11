using UnityEngine;

public class GameManager : MonoBehaviour{
    private bool GameEnded = false; 
    private bool GameWin = false; // Flag to check if the win UI is active
    public GameObject GameOverUI; // Reference to the Game Over UI
    public GameObject GameWinUI; // Reference to the Win UI

    void Start(){
        GameEnded = false; // Initialize GameEnded to false
        GameWin = false; // Initialize WinUI to false
    }
    // Update is called once per frame
    void Update(){
        if (GameEnded) return; // If the game is ended, do not process further
        if (GameWin) return; // If the game is won, do not process further

        if (PlayerStats.Lives <= 0) EndGame(); // Call EndGame if lives are 0 or less
        if(Input.GetKeyDown("e")) EndGame(); // Call EndGame when "E" is pressed
        if(Input.GetKeyDown("w")) WinGame(); // Call WinGame when "W" is pressed
        
    }
    void EndGame(){
        GameEnded = true;
        GameOverUI.SetActive(true); // Show the Game Over UI
        // Ensure the Game Over UI animation uses unscaled time
        //This allows the animation to play even when Time.timeScale = 0.
        //This is useful for UI animations that should not be affected by the game's time scale
        Animator animator = GameOverUI.GetComponent<Animator>();
        if (animator != null) animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        Debug.Log("Game Over!");

        // Pause the game
        Time.timeScale = 0;
    }

    public void WinGame(){
        GameWin = true;
        GameWinUI.SetActive(true); // Show the Win UI

        // Animator animator = GameWinUI.GetComponent<Animator>();
        // if (animator != null) animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        // Debug.Log("You Win!");


        // Pause the game
        Time.timeScale = 0;

        PlayerPrefs.SetInt("LevelProgress", PlayerPrefs.GetInt("LevelProgress") + 1); // Increment the level reached
        PlayerPrefs.Save(); // Save the updated level reached
    }
}
