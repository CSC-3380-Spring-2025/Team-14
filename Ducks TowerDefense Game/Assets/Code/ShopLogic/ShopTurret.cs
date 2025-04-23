using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour{

    [Header("Turret References")]
    public TurretBlueprint BlobTurret;
    public TurretBlueprint DuckTurret;
    public TurretBlueprint LaserBeamerTurret;
    public TurretBlueprint SnipeTurret;
    public TurretBlueprint GatlingTurret;
    public TurretBlueprint FreezeTurret;
    public TurretBlueprint NukeTurret;

    [Header("UI Buttons")]
    public Button gatlingButton;
    public Button freezeButton;
    public Button nukeButton;

    private PlaceTurret placeTurret;

    void Start(){
        placeTurret = PlaceTurret.instance;

        // Determine if turrets are unlocked based on PlayerPrefs
        GatlingTurret.isUnlocked = PlayerPrefs.GetInt("GatlingUnlocked", 0) == 1;
        FreezeTurret.isUnlocked = PlayerPrefs.GetInt("FreezeUnlocked", 0) == 1;
        NukeTurret.isUnlocked = PlayerPrefs.GetInt("NukeUnlocked", 0) == 1;

        // Update button states based on unlocked status
        if (gatlingButton) gatlingButton.interactable = GatlingTurret.isUnlocked;
        if (freezeButton) freezeButton.interactable = FreezeTurret.isUnlocked;
        if (nukeButton) nukeButton.interactable = NukeTurret.isUnlocked;
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