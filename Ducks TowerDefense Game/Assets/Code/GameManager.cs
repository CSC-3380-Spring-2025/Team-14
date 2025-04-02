using UnityEngine;

public class GameManager : MonoBehaviour
{
   private bool GameEnded = false; 

    // Update is called once per frame
    void Update()
    {
        if (GameEnded)
            return;
        
        if (PlayerStats.Lives <= 0){
            EndGame();
        }
    }
    void EndGame(){
        GameEnded = true;
        Debug.Log("Game Over!");
    }
}
