using System;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-100)]
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
            _money = value;
            OnMoneyChanged?.Invoke(_money);
            UpdateMoneyText();
            SaveMoney();
        }
    }

    [Header("UI Configuration")]
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
        LoadMoney();
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