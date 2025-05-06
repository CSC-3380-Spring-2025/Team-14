using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuShop : MonoBehaviour{ 
    [Header("References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI shopMoneyText;
    
    [Header("Turret Blueprints")]
    public TurretBlueprint GatlingTurret;
    public TurretBlueprint FreezeTurret;
    public TurretBlueprint NukeTurret;

    [Header("UI Buttons")]
    public Button gatlingButton;
    public Button freezeButton;
    public Button nukeButton;

    private Economy economy => Economy.Instance;

//Initializes turret unlock status and sets up the UI
    void Start(){
        if (Economy.Instance == null) return;
    
        GatlingTurret.isUnlocked = PlayerPrefs.GetInt("GatlingUnlocked", 0) == 1;
        FreezeTurret.isUnlocked = PlayerPrefs.GetInt("FreezeUnlocked", 0) == 1;
        NukeTurret.isUnlocked = PlayerPrefs.GetInt("NukeUnlocked", 0) == 1;
        
        // checks if the items have already been purchased
        gatlingButton.interactable = !GatlingTurret.isUnlocked;
        freezeButton.interactable = !FreezeTurret.isUnlocked;
        nukeButton.interactable = !NukeTurret.isUnlocked;

        economy.RefreshUI(moneyText);
        shopMoneyText.text = $"Coins: {economy.Money:N0}";
    }

    void Update(){
        if(Input.GetKeyDown("b")){
            PlayerPrefs.SetInt("GatlingUnlocked", 0);
            PlayerPrefs.SetInt("FreezeUnlocked", 0);
            PlayerPrefs.SetInt("NukeUnlocked", 0);

            gatlingButton.interactable = true;
            freezeButton.interactable = true;
            nukeButton.interactable = true;
        }
    }

// Purchase methods for each turret type
    public void PurchaseGatlingTurret() => PurchaseTurret(GatlingTurret, "GatlingUnlocked");
    public void PurchaseFreezeTurret() => PurchaseTurret(FreezeTurret, "FreezeUnlocked");
    public void PurchaseNukeTurret() => PurchaseTurret(NukeTurret, "NukeUnlocked");

    private void PurchaseTurret(TurretBlueprint turret, string saveKey){
        if (turret.cost > economy.Money || !economy.CanAfford(turret.cost)) return;
        
        economy.SpendMoney(turret.cost);
        turret.isUnlocked = true;
        PlayerPrefs.SetInt(saveKey, 1);
        PlayerPrefs.Save();

        economy.RefreshUI(moneyText);
        shopMoneyText.text = $"Coins: {economy.Money:N0}";

        // Disable buy button after purchase
        if (turret == GatlingTurret) gatlingButton.interactable = false;
        else if (turret == FreezeTurret) freezeButton.interactable = false;
        else if (turret == NukeTurret) nukeButton.interactable = false;
    }
}//End of MenuShop.cs