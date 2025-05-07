using UnityEngine;

public class UpgradeUIHandler : MonoBehaviour{
    public GameObject upgradeUI;
//Hides the upgrade UI when it starts
    private void Start(){
        if (upgradeUI != null) upgradeUI.SetActive(false); // Start hidden
        else Debug.LogWarning("UpgradeUI reference is missing!");
    }    
}//End of UpgradeUIHandler.cs