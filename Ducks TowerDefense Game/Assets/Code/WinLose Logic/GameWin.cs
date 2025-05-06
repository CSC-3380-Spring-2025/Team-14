using UnityEngine;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class GameWin : MonoBehaviour
{
    public TextMeshProUGUI roundsText; // Reference to the TextMeshProUGUI component for displaying rounds
    private GameManager gameManager; // Reference to the GameManager script

//--------------------------------------------------------------------
// Unity built-in method that is called when the object is instantiated
    void Awake(){
        // Find GameManager in the scene
        gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager == null) Debug.LogError("GameManager not found in the scene!");
    }
// Update the text with the number of rounds--------------------------------------------------------------------
    void OnEnable()
    {
        roundsText.text = PlayerStats.Rounds.ToString(); 
    }
//Reference in Unity, the onClick event of Continue button in GameWin UI--------------------------------------------------------------------
    
    public void ContinueGame()
    {
        // Reset the game state in GameManager
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ContinueGameState(); // Call a method in GameManager to reset the game state
        }

        // Simply unpause the game and close the win UI
        Time.timeScale = 1; // Resume the game
        gameObject.SetActive(false); // Hide the GameWin UI
    }
//Reference in Unity, the onClick event of NextMap button in GameWin UI--------------------------------------------------------------------
    
    public void NextMap(){
        if (gameManager != null) gameManager.ResetGame(); // Call a method in GameManager to reset the game state
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 >= totalScenes)
        {
            // If the current scene is the last one, return to the Map Selection menu
            SceneManager.LoadScene("Map Selection");
        }
        else
        {
            // Load the next scene in the build index
            SceneManager.LoadScene("Map Selection");
        }
    }
//Reference in Unity, the onClick event of Menu button in GameWin UI--------------------------------------------------------------------
    
    public void Menu(){
        Time.timeScale = 1f; // Unpause before leaving, Prevents Frozen UI in Next Scene
        //Full game reset if returning to menu
        if (gameManager != null) gameManager.ResetGame(); // Clear gameplay state
        SceneManager.LoadScene("Map Selection"); // Load the Map Selection menu
    }
//--------------------------------------------------------------------
}
