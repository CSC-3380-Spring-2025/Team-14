using UnityEngine;
using UnityEngine.UI;

public class WaveTimer : MonoBehaviour{
    public Text waveText;  // UI Text to show wave info
    public Button continueButton; // UI Button to start the next wave
    private int currentWave = 0; // Current wave number
    private Spawner spawner; // Reference to the Spawner script
    private ShopManager shopManager; // Reference to the ShopManager script
    private bool isWaveActive = false; // Flag to check if the wave is active, false means the wave is not active, true means the wave is active
    
    public Economy economy;


//Start is called once before the first execution of Update after the MonoBehaviour is created
//--------------------------------------------------------------------
    void Start(){
        // Initialize the wave text
        Debug.Log("WaveTimer initialized.");
        UpdateWaveText();

        // Find and link the Spawner script in the scene
        spawner = Object.FindAnyObjectByType<Spawner>();
        if(spawner == null) Debug.LogError("Spawner not found!");
        
        // Find and link the ShopManager script in the scene
        shopManager = Object.FindAnyObjectByType<ShopManager>();
        if(shopManager == null) Debug.LogError("ShopManager not found!");

        // Set up the button click event
        continueButton.onClick.AddListener(StartNewWave);
    }
//--------------------------------------------------------------------

//Update is called once per frame
//--------------------------------------------------------------------
    void Update() {
        // Check if the shop is open and update the continueButton state
        if (IsShopOpen()) {
            continueButton.interactable = false; // Disable interactivity
            continueButton.gameObject.SetActive(false); // Hide the button
        }
        else {
            // If the shop is closed, check if the shop panel has fully left the screen
            if (shopManager.HasShopPanelLeftScreen()) {
                // Show the continue button
                continueButton.gameObject.SetActive(true);

                // Check if the wave is active                
                continueButton.interactable = !isWaveActive;// Enable the button if the wave is not active, otherwise disable it
                                                            //!isWaveActive is equivalent to isWaveActive == false
                                                            //!True = False, !False = True

            }
            else {
                // If the shop panel is still moving, keep the button hidden
                continueButton.interactable = false;
                continueButton.gameObject.SetActive(false);
            }
        }
    }
//--------------------------------------------------------------------


//OnWaveDefeated is called when the current wave is defeated
//--------------------------------------------------------------------
    public void OnWaveDefeated(){
        if (!isWaveActive) return; // Exit if the wave is not active,
        // Check if the button is already enabled
        if (continueButton.interactable) return; // Exit if the button is already enabled
        
        // Enable the "Continue" button so the player can start the next wave
        Debug.Log("Wave defeated! Enabling Continue Button.");
        isWaveActive = false; // Wave is no longer active
        continueButton.interactable = true; // Enable the button
        OnRoundEnd(currentWave); //rewards the user with money
        
    }
//--------------------------------------------------------------------




//StartNewWave is called when the player clicks the "Continue" button, to start the next wave
//--------------------------------------------------------------------
    public void StartNewWave(){
        // Check if the button is already disabled
        if (!continueButton.interactable || IsShopOpen()) return;   // Exit if the button is already disabled
        

        Debug.Log("Starting new wave...");
        isWaveActive = true; // Wave is now active
        continueButton.interactable = false; // Disable the button until the next wave is defeated
        currentWave++; // Increase wave count
        Debug.Log($"Current wave: {currentWave}");


        // Start the new wave
        if (spawner != null){
            Debug.Log($"Calling StartWave for wave {currentWave}");
            spawner.StartWave(currentWave); // Tell the spawner to start a new wave
        }
        else{
            Debug.LogError("Spawner reference is missing!");
        }

        UpdateWaveText(); // Update the wave text
    }
//--------------------------------------------------------------------

public bool IsWaveActive() {
    return isWaveActive;
}

//IsShopOpen checks if the shop panel is open
//--------------------------------------------------------------------
    private bool IsShopOpen() {
        // Check if the shop is open by finding the ShopManager in the scene
        if (shopManager != null) {
            return shopManager.IsShopOpen();
        }
        return false;
    }
// --------------------------------------------------------------------


//UpdateWaveText updates the wave text UI element
//--------------------------------------------------------------------
    void UpdateWaveText(){
        waveText.text = $"Wave: {currentWave}";
    }
//--------------------------------------------------------------------
 // Method to give rewards to user after completing a wave. Rewards scale with it 
    void OnRoundEnd(int currentWave)
{
    int baseReward = 50;            // Starting amount
    float scalingFactor = 1.15f;    // How much it scales per round

    // Calculate reward: exponential growth
    int reward = Mathf.RoundToInt(baseReward * Mathf.Pow(scalingFactor, currentWave));

    economy.AddMoney(reward);
    Debug.Log($"Round {currentWave} complete! Earned ${reward}.");
}

}//End of WaveTimer class