using UnityEngine;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import the SceneManagement namespace

public class GameOver : MonoBehaviour{
    public TextMeshProUGUI roundsText; // Reference to the TextMeshProUGUI component for displaying rounds
    public Button retryButton; // Reference to the retry button
    public Button menuButton; // Reference to the menu button
    private GameManager gameManager; // Reference to the GameManager script
    
// Unity built-in method that is called when the object is instantiated
    void Awake(){
        // Find GameManager in the scene
        gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager == null) Debug.LogError("GameManager not found in the scene!");
    }

// Called when the GameOver UI becomes active
    void OnEnable(){
        roundsText.text = PlayerStats.Rounds.ToString(); // Update the text with the number of rounds
        retryButton.onClick.AddListener(Retry);
        menuButton.onClick.AddListener(Menu);
    }

// Called when the GameOver UI is disabled
    void OnDisable(){
        retryButton.onClick.RemoveListener(Retry);
        menuButton.onClick.RemoveListener(Menu);
    }

//Reference in Unity, the onClick event of Retry button in GameOver UI
    public void Retry(){
        Time.timeScale = 1f; // Unpause before retrying, Prevents Frozen UI in Next Scene
        gameManager.ResetGame(false);
        gameObject.SetActive(false);
    }

 //Reference in Unity, the onClick event of Menu button in GameOver UI
    public void Menu(){
        Time.timeScale = 1f; // Unpause before leaving, Prevents Frozen UI in Next Scene
        //Full game reset if returning to menu
        gameManager.ResetGame(); // Clear gameplay state
        SceneManager.LoadScene("Map Selection"); // Load the Main Menu scene
    }
}//End of GameOver.cs
