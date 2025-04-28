using TMPro;
using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy Instance { get; private set; }

    [SerializeField] private int initialMoney = 999999;
    private int _money;
    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _money = PlayerPrefs.GetInt("PlayerMoney", initialMoney);
        UpdateMoneyText();
    }

    public int Money => _money;

    public void AddMoney(int amount)
    {
        _money += amount;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", _money);
    }

    public bool CanAfford(int amount) => _money >= amount;

    public void SpendMoney(int amount)
    {
        if (!CanAfford(amount)) return;
        _money -= amount;
        UpdateMoneyText();
        PlayerPrefs.SetInt("PlayerMoney", _money);
    }

    public void RefreshUI(TextMeshProUGUI newMoneyText = null)
    {
        if (newMoneyText != null) moneyText = newMoneyText;
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null) moneyText.text = $"Coins: {_money:N0}";
    }
}