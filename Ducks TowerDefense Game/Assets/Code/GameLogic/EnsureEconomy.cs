using UnityEngine;

public class EnsureEconomy : MonoBehaviour
{
    void Awake()
    {
        if (Economy.Instance == null)
        {
            Debug.Log("Creating Economy instance in Map1");
            new GameObject("Economy").AddComponent<Economy>();
        }
    }
}