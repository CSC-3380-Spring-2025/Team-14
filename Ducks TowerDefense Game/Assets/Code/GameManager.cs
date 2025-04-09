using UnityEngine;

public class GameManager : MonoBehaviour
{
   private bool GameEnded = false; 
   public GameObject GameOverUI; // Reference to the Game Over UI

    void Start()
    {
        GameEnded = false; // Initialize GameEnded to false
    }
    // Update is called once per frame
    void Update()
    {
        if (GameEnded) return;
        
        
        if (PlayerStats.Lives <= 0){
            EndGame();
        }

        if(Input.GetKeyDown("e")){
            EndGame(); // Call EndGame when "E" is pressed
        }
    }
    void EndGame(){
        GameEnded = true;
        GameOverUI.SetActive(true); // Show the Game Over UI
        Debug.Log("Game Over!");
    }
}
