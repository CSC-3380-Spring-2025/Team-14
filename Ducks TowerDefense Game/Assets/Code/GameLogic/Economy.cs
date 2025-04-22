using System;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class Economy : MonoBehaviour
{
    public static Economy Instance { get; private set; }

    [Header("Economy Settings")]
    [SerializeField] private int initialMoney = 100;
    private int _money;

    public event Action<int> OnMoneyChanged; 

    public int Money
    {
        get => _money;
        private set
        {
            // Debug.Log("value is " + value);
            _money = value;
            OnMoneyChanged?.Invoke(_money);
            UpdateMoneyText();
            SaveMoney();
        }
    }

    [Header("UI Configuration")]
    public TextMeshProUGUI moneyText;

    private static bool _initialized = false;

    private void Awake()
    {
        if (_initialized && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _initialized = true;
        
        if (!PlayerPrefs.HasKey("PlayerMoney"))
        {
            PlayerPrefs.SetInt("PlayerMoney", initialMoney);
        }
        LoadMoney();
        
        Debug.Log($"Economy initialized in {gameObject.scene.name}");
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Coins: {_money:N0}";
        }
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public bool CanAfford(int amount) => Money >= amount;

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", Money);
        PlayerPrefs.Save();
    }

    private void LoadMoney()
    {
        Money = PlayerPrefs.GetInt("PlayerMoney", initialMoney);
    }

    public void RefreshUI(TextMeshProUGUI newMoneyText = null)
    {
        if (newMoneyText != null) moneyText = newMoneyText;
        UpdateMoneyText();
    }
}