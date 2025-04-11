using UnityEngine;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class GameWin : MonoBehaviour
{
    public TextMeshProUGUI roundsText; // Reference to the TextMeshProUGUI component for displaying rounds

    void OnEnable()
    {
        roundsText.text = PlayerStats.Rounds.ToString(); // Update the text with the number of rounds
    }

    public void ContinueGame()
    {
        // Simply unpause the game and close the win UI
        Time.timeScale = 1; // Resume the game
        gameObject.SetActive(false); // Hide the GameWin UI
    }

    public void NextMap()
    {
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
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Map Selection"); // Load the Map Selection menu
    }
}
