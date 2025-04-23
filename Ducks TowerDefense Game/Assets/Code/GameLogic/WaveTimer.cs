using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveTimer : MonoBehaviour
{
    [Header("UI References")]
    public Text waveText;  // UI Text to show wave info
    public Button continueButton; // UI Button to start the next wave

    [Header("Wave Settings")]
    [SerializeField] private int baseReward = 50;
    [SerializeField] private float rewardScalingFactor = 1.15f;

    private int currentWave = 0;
    public GameManager gameManager; // Reference to the GameManager
    private Spawner spawner;
    private ShopManager shopManager;
    private bool isWaveActive = false;
    private int lastLives = -1; 

    void Start()
    {
        if (continueButton != null) continueButton.onClick.AddListener(StartNewWave);
        
        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene reload
        Debug.Log("WaveTimer initialized.");
        UpdateWaveText();

        // Initialize references
        spawner = FindFirstObjectByType<Spawner>();
        if(spawner == null) Debug.LogError("Spawner not found!");
        
        shopManager = FindFirstObjectByType<ShopManager>();
        if(shopManager == null) Debug.LogError("ShopManager not found!");
    }

    void Update()
    {
        // Track lives changes
        int currentLives = PlayerStats.Lives;
        if (lastLives != -1 && currentLives < lastLives)
        {
            Debug.Log("Lives decreased during this wave.");
        }
        lastLives = currentLives;

        // Update button state based on shop status
        UpdateButtonState();
    }
    private void OnEnable()
    {
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false); // Ensure button is hidden on scene restart
            continueButton.interactable = false;
        }
    }


    private void UpdateButtonState()
    {
        if (continueButton == null || shopManager == null) return;

        if (IsShopOpen())
        {
            continueButton.interactable = false;
            continueButton.gameObject.SetActive(false);
            return;
        }
        else if (shopManager.HasShopPanelLeftScreen())
        {
            continueButton.gameObject.SetActive(true);
            continueButton.interactable = !isWaveActive;
        }
        else
        {
            continueButton.interactable = false;
            continueButton.gameObject.SetActive(false);
        }
    }

    public void OnWaveDefeated()
    {
        if (!isWaveActive || continueButton == null || continueButton.interactable) return;
        
        Debug.Log("Wave defeated! Enabling Continue Button.");
        isWaveActive = false;
        continueButton.interactable = true;
        AwardWaveCompletionReward();

        // Check if the current wave is the last wave
        if (currentWave % 5 == 0 && currentWave > 0){
            Debug.Log("All waves defeated! Player wins the game.");
            gameManager.WinGame(); // Call the WinLevel method in GameManager
        }
    }

    public void StartNewWave()
    {
        if (!continueButton.interactable || IsShopOpen() || isWaveActive) return;
        
        Debug.Log("Starting new wave...");
        isWaveActive = true;
        continueButton.interactable = false;
        currentWave++;
        PlayerStats.Rounds++;
        Debug.Log($"Current wave: {currentWave}");

        if (spawner != null){
            Debug.Log($"Calling StartWave for wave {currentWave}");
            spawner.StartWave(currentWave);
        }
        else Debug.LogError("Spawner reference is missing!");

        UpdateWaveText();

        
        
    }

    private void AwardWaveCompletionReward(){
        if (Economy.Instance == null) return;
        
        
        int reward = Mathf.RoundToInt(baseReward * Mathf.Pow(rewardScalingFactor, currentWave));
        // int reward = 0;
        Debug.Log(reward);
        Economy.Instance.AddMoney(reward);
    }

    public bool IsWaveActive() => isWaveActive;

    private bool IsShopOpen() => shopManager != null && shopManager.IsShopOpen();

    private void UpdateWaveText() => waveText.text = $"Wave: {currentWave}";

}