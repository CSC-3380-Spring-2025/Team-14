using UnityEngine;

public class Shop : MonoBehaviour{

    public TurretBlueprint BlobTurret;
    public TurretBlueprint DuckTurret;
    public TurretBlueprint LaserBeamerTurret;
    public TurretBlueprint SnipeTurret;
    public TurretBlueprint GatlingTurret;
    public TurretBlueprint FreezeTurret;
    public TurretBlueprint NukeTurret;
    PlaceTurret placeTurret;
    void Start(){
        placeTurret = PlaceTurret.instance;
    }
    public void purchaseBlobTurret(){
        Debug.Log("Turret Selected");
        placeTurret.selectTurretToPlace(BlobTurret);
    }
    public void purchaseDuckTurret(){
        Debug.Log("Turret Selected 2");
        placeTurret.selectTurretToPlace(DuckTurret);

    }
    
    public void purchaseLaserBeamerTurret(){
        Debug.Log("Turret Selected 3");
        placeTurret.selectTurretToPlace(LaserBeamerTurret);

    }
    public void purchaseSnipeTurret(){
        Debug.Log("Turret Selected 4");
        placeTurret.selectTurretToPlace(SnipeTurret);

    }
    public void purchaseGatlingTurret(){
        Debug.Log("Turret Selected 5");
        placeTurret.selectTurretToPlace(GatlingTurret);
    }
    public void purchaseFreezeTurret(){
        Debug.Log("Turret Selected 6");
        placeTurret.selectTurretToPlace(FreezeTurret);
    }
    public void purchaseNukeTurret(){
        Debug.Log("Nuke Selected");
        placeTurret.selectTurretToPlace(NukeTurret);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}