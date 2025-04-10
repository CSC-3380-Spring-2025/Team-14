using UnityEngine;

public class Shop : MonoBehaviour{

    public TurretBlueprint BlobTurret;
    public TurretBlueprint DuckTurret;
    public TurretBlueprint LaserBeamerTurret;
    public TurretBlueprint SnipeTurret;
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
        Debug.Log("Turret Selected 3");
        placeTurret.selectTurretToPlace(SnipeTurret);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}