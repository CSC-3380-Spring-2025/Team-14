using UnityEngine;

public class Shop : MonoBehaviour{

    PlaceTurret placeTurret;
    void Start(){
        placeTurret = PlaceTurret.instance;
    }
    public void BlobTurret(){
        Debug.Log("Turret Selected");
        placeTurret.setTurretToBuild(placeTurret.BlobTurretPrefab);
    }
    public void DuckTurret(){
        Debug.Log("Turret Selected 2");
        placeTurret.setTurretToBuild(placeTurret.DuckTurretPrefab);

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}