using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour{

    public Color hoverColor;
    public Vector3 positionOffSet;//Set the position of turret in the GameObject(square)
    public Vector3 rotationOffSet;//set the rotation og turret in the GameObject(square)
    public GameObject turret;
    public Turret _turret; 
    private SpriteRenderer  rend;
    private Color startColor;
    PlaceTurret placeTurret; //reference PlaceTurret Script
//Initalizes the node's renderer, color and gets a reference to the PlaceTurret--------------------------------------------------------------------
    void Start(){
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.material.color;
        placeTurret = PlaceTurret.instance;
        
    }
//Calculates where the turret will be placed--------------------------------------------------------------------

    public Vector3 GetPlacePosition()
    {
        return transform.position + positionOffSet;
    }
//Handels the mouse click on the node. If there is no turret there it places it.--------------------------------------------------------------------
//If a turret is there it opens the upgade UI
// If a turret is there it doesnt let you place it there 
    void OnMouseDown(){
        if(UIManager.main.IsHoveringUI()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;
            
        
        
    
        
        if(turret != null){
            _turret.OpenUpgradeUI();
            return;
        }
        
        if(!placeTurret.CanPlace) return;

        //build a turret
        //reference PlaceTurret
        //stores placeTurret.BuildTurret() into turretBuilding and instantiate it
        placeTurret.PlaceTurretOn(this);
        _turret = turret.GetComponent<Turret>();
        _turret.OpenUpgradeUI();
        
    }
//--------------------------------------------------------------------

    // In order for this effect to work, I would need to add a Box Collider Component in the inspector of the game Object (Square)
//Changes the node color when a mouse enters if you are allowed to place it there--------------------------------------------------------------------
    void OnMouseEnter(){ //OnMouseEnter - Enter the node
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if(!placeTurret.CanPlace) return;
        rend.sharedMaterial.color = hoverColor;
    }
//Restores the node original color--------------------------------------------------------------------
    void OnMouseExit(){ //OnMouseExit - Exit the node
    if (rend != null)
        rend.material.color = startColor;  
    }
//--------------------------------------------------------------------

}
