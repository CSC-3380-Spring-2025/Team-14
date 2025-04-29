using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveTimer : MonoBehaviour
{
    [Header("UI References")]
    public Text waveText;
    public Button continueButton;

    [Header("Wave Settings")]
    [SerializeField] private int baseReward = 50;
    [SerializeField] private float rewardScalingFactor = 1.15f;
    private GameManager gameManager;

    private Spawner spawner;
    private ShopManager shopManager;
    public bool isWaveActive = false;
    private int currentWave = 0;
    private int lastLives = -1;

    [Header("Path Indicator")]
    public GameObject pathIndicatorPrefab;
    public Vector2 indicatorUIPosition = new Vector2(13.5f, -4.5f); // Set this in Inspector
    private GameObject pathIndicatorInstance;
    private bool firstWave = true;

    void Start()
    {
        if (continueButton != null) continueButton.onClick.AddListener(StartNewWave);

        DontDestroyOnLoad(gameObject);
        UpdateWaveText();

        spawner = FindFirstObjectByType<Spawner>();
        if (spawner == null) Debug.LogError("Spawner not found!");

        shopManager = FindFirstObjectByType<ShopManager>();
        if (shopManager == null) Debug.LogError("ShopManager not found!");

        // Show the UI path indicator before the first wave
        if (firstWave && pathIndicatorPrefab != null && pathIndicatorInstance == null)
        {
            GameObject canvas = GameObject.Find("Canvas"); // Make sure your Canvas is named "Canvas"
            if (canvas != null)
            {
                pathIndicatorInstance = Instantiate(pathIndicatorPrefab, canvas.transform);

                RectTransform rt = pathIndicatorInstance.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = indicatorUIPosition;
                    rt.localRotation = Quaternion.Euler(0f, 0f, 270f); // Rotate the arrow
                }
            }
            else Debug.LogError("Canvas not found in the scene!");
            
        }
    }

    void Update()
    {
        int currentLives = PlayerStats.Lives;
        if (lastLives != -1 && currentLives < lastLives) Debug.Log("Lives decreased during this wave.");
        
        lastLives = currentLives;

        UpdateButtonState();
    }

    private void OnEnable()
    {
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
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

        isWaveActive = false;
        continueButton.interactable = true;
        AwardWaveCompletionReward();

        if (currentWave % 10 == 0 && currentWave > 0) gameManager.WinGame();
        
    }

    public void StartNewWave()
    {
        if (!continueButton.interactable || IsShopOpen() || isWaveActive) return;

        isWaveActive = true;
        continueButton.interactable = false;
        currentWave++;
        PlayerStats.Rounds++;

        // Fade out or destroy indicator
        if (pathIndicatorInstance != null) Destroy(pathIndicatorInstance);
        firstWave = false;
        

        if (spawner != null) spawner.StartWave(currentWave);
        

        UpdateWaveText();
    }

    private void AwardWaveCompletionReward()
    {
        if (Economy.Instance == null) return;

        int reward = Mathf.RoundToInt(baseReward * Mathf.Pow(rewardScalingFactor, currentWave));
        Economy.Instance.AddMoney(reward);
    }

    // Add the ResetWaves method
    public void ResetWaves()
    {
        Debug.Log("Waves have been reset.");
        // Add logic to reset waves if necessary
        currentWave = 0;
        isWaveActive = false;
        UpdateWaveText();

        if (spawner != null) spawner.ResetSpawner();
        
        firstWave = true; // Reset firstWave to true
        lastLives = 3; // Reset lastLives to 3
    }

    public bool IsWaveActive() => isWaveActive;
    private bool IsShopOpen() => shopManager != null && shopManager.IsShopOpen();
    private void UpdateWaveText() => waveText.text = $"Wave: {currentWave}";
}
