using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour{

    [Header("Turret References")]
    public TurretBlueprint BaseTurret;
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

//Initializes turret unlock status and updates UI buttons
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

// Player selects Base turret
    public void purchaseBlobTurret() => placeTurret.selectTurretToPlace(BaseTurret);
    
// Player selects aoe turret
    public void purchaseDuckTurret() => placeTurret.selectTurretToPlace(DuckTurret);

// Player selects laser turret
    public void purchaseLaserBeamerTurret() => placeTurret.selectTurretToPlace(LaserBeamerTurret);

// Player selects Sniper turret
    public void purchaseSnipeTurret() => placeTurret.selectTurretToPlace(SnipeTurret);

// Player selects gatling turret
    public void purchaseGatlingTurret() => placeTurret.selectTurretToPlace(GatlingTurret);
    
// Player selects freeze turret
    public void purchaseFreezeTurret() => placeTurret.selectTurretToPlace(FreezeTurret);
    
// Player selects Nuke turret
    public void purchaseNukeTurret() => placeTurret.selectTurretToPlace(NukeTurret);
}//End of ShopTurret.cs