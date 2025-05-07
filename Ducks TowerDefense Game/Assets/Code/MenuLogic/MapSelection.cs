using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// This script handles the map selection and progress tracking in the game.
public class MapSelection : MonoBehaviour{
    public string mapSelected;
    public Button clickableButton;
    //Tracker for original selection
    public int oldIndex = -1;
    public int oldDifIndex = -1;

    // Map Image Array
    public GameObject[] mapImages;
    // Difficulty Stars Array (Implementation Needed, plan on using tags)
    public GameObject[] difficultyStars;
    // Map buttons array
    public Button[] mapButton;

//Initializes map buttons, loads player progress, sets up button listeners--
    void Start(){
        clickableButton.interactable = false;

        // Check progress and lock/unlock maps
        int progress = PlayerPrefs.GetInt("LevelProgress", 1); // Default to 1 (Map 1 unlocked)

        for (int i = 0; i < mapButton.Length; i++){
            // Unlock the button if progress is greater than or equal to the map index (1-based)
            mapButton[i].interactable = (i + 1) <= progress;

            // Add a listener to each button to select the map
            int index = i; // Capture the current index for the lambda
            
            mapButton[i].onClick.AddListener(() =>{
                //Assuming not initial click, deactivates map preview
                if (oldIndex != -1)
                {
                    mapImages[oldIndex].SetActive(false);
                }

                mapSelected = "Map" + (index + 1); // Set the selected map
                clickableButton.interactable = true; // Enable the PlayMap button
                mapImages[index].SetActive(true); //Activates selected map preview
                oldIndex = index; //store old index
                Difficulty(mapButton[index]);
            });
        }
    }
// //Listens for player input to reset map progess this is for the live demo.This might not be needed
    // void Update(){
    //     // Reset map lock to default when "R" is pressed
    //     if (Input.GetKeyDown(KeyCode.R)){
    //         Debug.Log("Resetting map locks to default...");
    //         PlayerPrefs.SetInt("LevelProgress", 1); // Reset progress to default (Map 1 unlocked)
    //         PlayerPrefs.Save();

    //         // Update map buttons to reflect the reset state
    //         for (int i = 0; i < mapButton.Length; i++){
    //             mapButton[i].interactable = (i == 0); // Only Map 1 is unlocked
    //         }
    //         clickableButton.interactable = false; // Disable the PlayMap button
    //         // Reset the game state using GameManager
    //         GameManager gameManager = FindFirstObjectByType<GameManager>();
    //         if (gameManager != null) gameManager.ContinueGameState(); // Reset the game state
    //     }
    // }

//Loads the selected map and resets the game before starting the new map
    public void PlayMap(){
        // Reset the game state before starting the map
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null) gameManager.ContinueGameState(); // Reset the game state
        if (!string.IsNullOrEmpty(mapSelected)) SceneManager.LoadScene(mapSelected);
        else Debug.Log("No Map Selected");
    }

//Unlocks the next map if the map before was won
    public void CompleteMap(string completedMap){
        int progress = PlayerPrefs.GetInt("LevelProgress", 1);
        if (completedMap == "Map1" && progress < 2) PlayerPrefs.SetInt("LevelProgress", 2); // Unlock Map 2
        else if (completedMap == "Map2" && progress < 3) PlayerPrefs.SetInt("LevelProgress", 3); // Unlock Map 3
        PlayerPrefs.Save();
    }

//Returns to the main menu
    public void goToMainMenu() => SceneManager.LoadScene("MainMenu");

//Checks the tag of the button to determine the difficulty
    public string checkTag(Button objectToCheck){
        string objectTag = objectToCheck.tag;
        return objectTag;
    }

//Shows the difficulty stars based on the button's tag
    public void showDif(Button objectToCheck){
        string objectTag = checkTag(objectToCheck);

        if (string.Compare(objectTag, "EasyDif") == 0){
            difficultyStars[0].SetActive(true);
            oldDifIndex = 0;
        }
        else if (string.Compare(objectTag, "NormalDif") == 0){
            difficultyStars[1].SetActive(true);
            oldDifIndex = 1;
        }
        else if (string.Compare(objectTag, "HardDif") == 0){
            difficultyStars[2].SetActive(true);
            oldDifIndex = 2;
        }
        else{
            oldDifIndex = -1;
            return;
        }
    }

//Turns off the old difficulty stars
    public void turnOffOldDif(int index){
        if (oldDifIndex != -1) difficultyStars[index].SetActive(false);
        else return;
    }

////Handles the difficulty selection and updates the UI accordingly
    public void Difficulty(Button objectToUse){
        turnOffOldDif(oldDifIndex);
        showDif(objectToUse);
    }
}//End of MapSelection.cs
