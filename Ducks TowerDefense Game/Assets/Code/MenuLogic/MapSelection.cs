using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// This script handles the map selection and progress tracking in the game.
public class MapSelection : MonoBehaviour
{
    public string mapSelected;
    public Button clickableButton;

    // Map buttons array
    public Button[] mapButton;

    void Start()
    {
        clickableButton.interactable = false;

        // Check progress and lock/unlock maps
        int progress = PlayerPrefs.GetInt("LevelProgress", 1); // Default to 1 (Map 1 unlocked)

        for (int i = 0; i < mapButton.Length; i++)
        {
            // Unlock the button if progress is greater than or equal to the map index (1-based)
            mapButton[i].interactable = (i + 1) <= progress;

            // Add a listener to each button to select the map
            int index = i; // Capture the current index for the lambda
            mapButton[i].onClick.AddListener(() =>
            {
                mapSelected = "Map" + (index + 1); // Set the selected map
                clickableButton.interactable = true; // Enable the PlayMap button
            });
        }
    }

    void Update()
    {
        // Reset map lock to default when "R" is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Resetting map locks to default...");
            PlayerPrefs.SetInt("LevelProgress", 1); // Reset progress to default (Map 1 unlocked)
            PlayerPrefs.Save();

            // Update map buttons to reflect the reset state
            for (int i = 0; i < mapButton.Length; i++)
            {
                mapButton[i].interactable = (i == 0); // Only Map 1 is unlocked
            }

            clickableButton.interactable = false; // Disable the PlayMap button

            // Reset the game state using GameManager
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.ContinueGameState(); // Reset the game state
            }
        }
    }

    public void PlayMap()
    {
        // Reset the game state before starting the map
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ContinueGameState(); // Reset the game state
        }

        if (!string.IsNullOrEmpty(mapSelected))
        {
            SceneManager.LoadScene(mapSelected);
        }
        else
        {
            Debug.Log("No Map Selected");
        }
    }

    public void CompleteMap(string completedMap)
    {
        int progress = PlayerPrefs.GetInt("LevelProgress", 1);

        if (completedMap == "Map1" && progress < 2)
        {
            PlayerPrefs.SetInt("LevelProgress", 2); // Unlock Map 2
        }
        else if (completedMap == "Map2" && progress < 3)
        {
            PlayerPrefs.SetInt("LevelProgress", 3); // Unlock Map 3
        }

        PlayerPrefs.Save();
    }

    //Do Not Delete, Loads Main Menu -_-
    //ok :) My bad :3
    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }

}
