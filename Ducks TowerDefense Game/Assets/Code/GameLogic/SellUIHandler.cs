using UnityEngine;

public class SellUIHandler : MonoBehaviour{
    public GameObject sellUI;
//Hides the sell UI when the game starts
    private void Start(){
        if (sellUI != null) sellUI.SetActive(false); // Start hidden
        else Debug.LogWarning("SellUI reference is missing!");
    }    
}//End of SellUIHandler.cs
