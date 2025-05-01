using UnityEngine;
using UnityEngine.UI;

//Map currency and map lives script
// This script manages the player's stats, including money and lives, and updates the UI accordingly.
public class PlayerStats : MonoBehaviour {
    [Header("UI References")]
    public Text moneyText; // Reference to the money UI text
    public Text livesText; // Reference to the lives UI text
    public static int Rounds;

    // Player stats
    [Header("Player Stats")]
    public static int startMoney = 500; // Initial money
    public static int startLives = 3;  // Initial lives

    private static int money;
    public static int Money {
        get { return money; }
        set {
            Debug.Log($"Money updated: {value}"); // Debug log to check the value
            money = Mathf.Max(0, value); // Prevent negative money
            OnMoneyChanged?.Invoke(money); // Trigger the money changed event
        }
    }

    private static int lives;
    public static int Lives {
        get { return lives; }
        set {
            lives = Mathf.Max(0, value); // Prevent negative lives
            OnLivesChanged?.Invoke(lives); // Trigger the lives changed event
            if (lives <= 0) {
                // Handle game over logic here
                Debug.Log("Game Over!");
            }
        }
    }

    // Events to notify when money or lives change
    public static event System.Action<int> OnMoneyChanged;
    public static event System.Action<int> OnLivesChanged;

    private void Start() {
        // Initialize stats
        Money = startMoney;
        Lives = startLives;

        // Subscribe to events
        OnMoneyChanged += UpdateMoneyText;
        OnLivesChanged += UpdateLivesText;

        // Update UI initially
        UpdateMoneyText(Money);
        UpdateLivesText(Lives);
    }

    private void OnDestroy() {
        // Unsubscribe from events to avoid memory leaks
        OnMoneyChanged -= UpdateMoneyText;
        OnLivesChanged -= UpdateLivesText;
    }

    private void UpdateMoneyText(int currentMoney) {
        if (moneyText == null) {
            Debug.LogError("MoneyText is not assigned in the inspector.");
            return;
        }
        Debug.Log("current money is " + currentMoney);
        moneyText.text = $"{currentMoney}";
    }

    private void UpdateLivesText(int currentLives) {
        if (livesText == null) {
            Debug.LogError("LivesText is not assigned in the inspector.");
            return;
        }
        livesText.text = $"Lives: {currentLives}";
    }

    public static void ResetAll(){
        Money = 999999;
        Lives = 3;
        Rounds = 0;
    }
}
