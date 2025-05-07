using UnityEngine;

public class UIManager : MonoBehaviour{
//this script might not be needed. 
    public static UIManager main;
    public bool isHoveringUI;

//Called when the script instance is being loaded
    public void Awake() => main = this;
    
//sets the hovering state
    public void SetHoveringState(bool state) => isHoveringUI = state;
    
// returns if the player is hovering the UI
    public bool IsHoveringUI() => isHoveringUI;
}//End of UIManager.cs
