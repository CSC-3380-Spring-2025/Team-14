using UnityEngine;
using UnityEngine.UI;

public class WaveTimer : MonoBehaviour{
    public Text waveText;  // UI Text to show wave info
    public Button continueButton; // UI Button to start the next wave
    private int currentWave = 0; // Current wave number
    private Spawner spawner; // Reference to the Spawner script

    private bool isWaveActive = false;



//Start is called once before the first execution of Update after the MonoBehaviour is created
//--------------------------------------------------------------------
    void Start(){
        // Initialize the wave text
        Debug.Log("WaveTimer initialized.");
        UpdateWaveText();

        // Find and link the Spawner script in the scene
        spawner = Object.FindAnyObjectByType<Spawner>();
        if(spawner == null) Debug.LogError("Spawner not found!");
        
        // Set up the button click event
        continueButton.onClick.AddListener(StartNewWave);
    }
//--------------------------------------------------------------------




//OnWaveDefeated is called when the current wave is defeated
//--------------------------------------------------------------------
    public void OnWaveDefeated(){
        // Check if the button is already enabled
        if (continueButton.interactable) return; // Exit if the button is already enabled
        
        // Enable the "Continue" button so the player can start the next wave
        Debug.Log("Wave defeated! Enabling Continue Button.");
        continueButton.interactable = true; // Enable the button
        isWaveActive = false;

          if (continueButton != null)
    {
        Debug.LogWarning("Showing Start Button because wave is over.");
        continueButton.gameObject.SetActive(true);
    }
    }
//--------------------------------------------------------------------




//StartNewWave is called when the player clicks the "Continue" button, to start the next wave
//--------------------------------------------------------------------
    public void StartNewWave(){
        // Check if the button is already disabled
        if (!continueButton.interactable) return;   // Exit if the button is already disabled
        

        Debug.Log("Starting new wave...");
        continueButton.interactable = false; // Disable the button until the next wave is defeated

        // Increase wave count
        currentWave++; 
        isWaveActive = true;
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

        if (continueButton != null)
    {
        Debug.LogWarning("Hiding Start Button because wave started.");
        continueButton.gameObject.SetActive(false);
    }
        
    }
//--------------------------------------------------------------------





//UpdateWaveText updates the wave text UI element
//--------------------------------------------------------------------
    void UpdateWaveText(){
        waveText.text = $"Wave: {currentWave}";
    }
//--------------------------------------------------------------------


}//End of WaveTimer class