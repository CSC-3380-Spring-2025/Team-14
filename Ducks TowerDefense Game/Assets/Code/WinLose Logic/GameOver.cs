using UnityEngine;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI roundsText; // Reference to the TextMeshProUGUI component for displaying rounds

    void OnEnable(){
        roundsText.text = PlayerStats.Rounds.ToString(); // Update the text with the number of rounds
    }

    public void Retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
    public void Menu(){
        SceneManager.LoadScene("Map Selection"); // Load the Main Menu scene
    }
}
