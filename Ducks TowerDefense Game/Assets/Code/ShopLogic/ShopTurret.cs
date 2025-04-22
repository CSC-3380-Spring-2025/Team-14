using UnityEngine;

public class Shop : MonoBehaviour{

    public TurretBlueprint BlobTurret;
    public TurretBlueprint DuckTurret;

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
    

    // Update is called once per frame
    void Update()
    {
        
    }
}