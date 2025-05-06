using TMPro;
using UnityEngine;

public class Economy : MonoBehaviour{

    // ========== Basic Variables ==========
    // Singleton instance
    public static Economy Instance { get; private set; }
    [SerializeField] private int initialMoney = 500;
    private int _money; // current money 
    public TextMeshProUGUI moneyText;


//Loads saved money and updates the UI
    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _money = PlayerPrefs.GetInt("PlayerMoney", initialMoney);
        UpdateMoneyText();
    }

//Get the current money
    public int Money => _money;

//This is used to add money to the player
    public void AddMoney(int amount){
        _money += amount;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", _money);
    }

//Checks if the player can afford the item
    public bool CanAfford(int amount) => _money >= amount;

    public void SpendMoney(int amount){
        if (!CanAfford(amount)) return;
        _money -= amount;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", _money);
    }
//This refreshes the money UI if needed
    public void RefreshUI(TextMeshProUGUI newMoneyText = null){
        if (newMoneyText != null) moneyText = newMoneyText;
        UpdateMoneyText();
    }
//This updates the displayed money text in the UI
    private void UpdateMoneyText(){
        if (moneyText != null) moneyText.text = $"Coins: {_money:N0}";
    }
}//End of Economy.cs