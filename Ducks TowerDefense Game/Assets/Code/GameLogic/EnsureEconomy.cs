using UnityEngine;

public class EnsureEconomy : MonoBehaviour
{
//Checks if the economy exists. If not it makes a new economy game object--------------------------------------------------------------------
    void Awake()
    {
        if (Economy.Instance == null)
        {
            Debug.Log("Creating Economy instance in Map1");
            new GameObject("Economy").AddComponent<Economy>();
        }
    }
//--------------------------------------------------------------------
}