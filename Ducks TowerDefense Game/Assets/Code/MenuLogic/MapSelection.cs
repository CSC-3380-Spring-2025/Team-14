using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelection : MonoBehaviour
{

    public string mapSelected;
    public Button clickableButton;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickableButton.interactable = false;
    }

    public void PlayMap()
    {
        if (mapSelected == null)
        {
            Debug.Log("No Map Selected");
        }
        else
        {
            SceneManager.LoadScene(mapSelected);
        }
        
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void selectMap()
    {
        //Checking Current Map Selected if it matches. Deselects the map.
        if (mapSelected == null || mapSelected != EventSystem.current.currentSelectedGameObject.name)
        {
            mapSelected = EventSystem.current.currentSelectedGameObject.name;
            clickableButton.interactable = SceneExistsInBuild(mapSelected);

        }

        else
        {
            mapSelected = null;
            clickableButton.interactable = false;
        }
        Debug.Log(mapSelected);
    }

    public bool SceneExistsInBuild(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
