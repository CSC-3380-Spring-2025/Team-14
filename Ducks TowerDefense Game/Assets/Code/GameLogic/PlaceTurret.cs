using UnityEngine;

public class PlaceTurret : MonoBehaviour{

    /*Singleton Pattern:
    *Creating a method to access building turret without referencing PlaceTurret in the Node.cs Script
    *Reason:   Easier to deal with, if I had reference it to the Node.cs Script
    *          This may cause a lot of errors or will be very annoying to deal with
    *          The reason it will be annoying to deal with is because every single gameobject(square) will have a implmenation of PlaceTurret
    *Public allow access without the class, and static because we want it be access only by PlaceTurret.cs 
    *This variable is a PlaceTurret inside a PlaceTurret. Basically stores a reference to itself
    */
    public static PlaceTurret instance;
    public Node LastNodeWithUI;
    void Awake(){
        if(instance != null){
            Debug.LogError("More than once PlaceTurret in scene");
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }
        instance = this;//everythime we start the game there is going to be one PlaceTurret 
                        //It going to call the awake method and store PlaceTurret in the instance variable
                        //This allows the instance variable to be access anywhere in PlaceTurret.cs Script
        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene reload
        Debug.Log("PlaceTurret Singleton initialized.");
    }



    
    private TurretBlueprint turretBuilding;

    public bool CanPlace { get { return turretBuilding != null; } }


    void Update() {
        if (Input.GetMouseButtonDown(0) && !UIManager.main.IsHoveringUI()) {
            if (LastNodeWithUI != null) {
                LastNodeWithUI._turret.CloseUpgradeUI();
                LastNodeWithUI = null;
            }
        }
    }
    public void PlaceTurretOn(Node node)
    {
        if (PlayerStats.Money < turretBuilding.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }
        PlayerStats.Money -= turretBuilding.cost;
        // Apply position AND rotation offset from the Node
        Quaternion rotation = Quaternion.Euler(node.rotationOffSet); // Fix typo in variable name
        GameObject turret = (GameObject)Instantiate(turretBuilding.prefab, node.GetPlacePosition(), transform.rotation);
        node.turret = turret;

        Debug.Log ("Money left after building: " + PlayerStats.Money);
        turretBuilding = null;
    }

    //You can choice which turret to build

    public void selectTurretToPlace(TurretBlueprint turret) {
        turretBuilding = turret;
    }
   
}