using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Currently will load the first map and immediately play it
    //Goal of this is to load a map selector and estart the game
    public void playGame() {
        SceneManager.LoadScene("Map Selection");
    }

    //Quits the game
    public void quitGame() {
        //Debug code one line down, remove in final
        Debug.Log("QUIT!");

        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
