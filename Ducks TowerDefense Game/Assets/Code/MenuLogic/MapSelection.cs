using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void PlayMap()
    {
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
    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
