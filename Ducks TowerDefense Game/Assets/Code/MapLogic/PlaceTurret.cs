using UnityEngine;

public class PlaceTurret : MonoBehaviour{

    //Singleton Pattern:
    //Creating a method to access building turret without referencing PlaceTurret in the Node.cs Script
    //Reason:   Easier to deal with, if I had reference it to the Node.cs Script
    //          This may cause a lot of errors or will be very annoying to deal with
    //          The reason it will be annoying to deal with is because every single gameobject(square) will have a implmenation of PlaceTurret
    //Public allow access without the class, and static because we want it be access only by PlaceTurret.cs 
    //This variable is a PlaceTurret inside a PlaceTurret. Basically stores a reference to itself
    public static PlaceTurret instance;
    void Awake(){
        if(instance != null){
            Debug.LogError("More than once PlaceTurret in scene");
            return;
        }
        instance = this;//everythime we start the game there is going to be one PlaceTurret 
                        //It going to call the awake method and store PlaceTurret in the instance variable
                        //This allows the instance variable to be access anywhere in PlaceTurret.cs Script
    }




    public GameObject BlobTurretPrefab;
    public GameObject DuckTurretPrefab;  //Turret
    private GameObject turretBuilding;


    public GameObject BuildTurret(){
        return turretBuilding;
    }

    //You can choice which turret to build
    public void setTurretToBuild(GameObject turret){
        turretBuilding = turret;
    }
   
}