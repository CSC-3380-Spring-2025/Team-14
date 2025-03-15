using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void PlayMap1()
    {
        SceneManager.LoadScene("Map1");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
