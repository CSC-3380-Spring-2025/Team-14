// Add this to a new script called "Preloader.cs"
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreLoader : MonoBehaviour 
{
//--------------------------------------------------------------------
    void Start(){
        Debug.Log($"Current Economy instance: {Economy.Instance}");
    }
//--------------------------------------------------------------------
    void Awake(){
        // Force Economy to initialize
        var economy = Economy.Instance;
        SceneManager.LoadScene(1); // Load MainMenu next
    }
//--------------------------------------------------------------------
}