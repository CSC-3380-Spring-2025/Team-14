using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UpgradeUIHandler : MonoBehaviour
{
    public GameObject upgradeUI;

    private void Start()
    {
        if (upgradeUI != null)
            upgradeUI.SetActive(false); // Start hidden
        else
            Debug.LogWarning("UpgradeUI reference is missing!");
    }

    private void OnMouseEnter()
    {
        Debug.Log("MOUSE ENTERED!");
        if (upgradeUI != null)
            upgradeUI.SetActive(true);
    }

    private void OnMouseExit()
    {
        Debug.Log("MOUSE EXITED!");
        if (upgradeUI != null)
            upgradeUI.SetActive(false);
    }
}