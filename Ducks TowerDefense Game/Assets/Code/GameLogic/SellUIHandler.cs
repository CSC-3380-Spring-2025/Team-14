using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellUIHandler : MonoBehaviour
{
    public GameObject sellUI;

    private void Start()
    {
        if (sellUI != null)
            sellUI.SetActive(false); // Start hidden
        else
            Debug.LogWarning("SellUI reference is missing!");
    }

    private void OnMouseEnter()
    {
        Debug.Log("MOUSE ENTERED!");
        if (sellUI != null)
            sellUI.SetActive(true);
    }

    private void OnMouseExit()
    {
        Debug.Log("MOUSE EXITED!");
        if (sellUI != null)
            sellUI.SetActive(false);
    }
}
