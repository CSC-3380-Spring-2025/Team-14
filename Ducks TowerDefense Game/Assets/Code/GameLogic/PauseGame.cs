using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    // [SerializeField] is an attribute in Unity that allows you to keep private variables visible and editable in the Inspector while maintaining encapsulation in your code.
    [SerializeField] private RectTransform PausePanel;  // Reference to the Pause Panel
    [SerializeField] private Button PauseButton;       // Reference to the Pause Button
    [SerializeField] private float slideSpeed = 1000f;  // Speed of the slide animation

    private Vector2 offScreenPosition;
    private Vector2 onScreenPosition;
    private bool ifPause = false;

    private GameManager gameManager; // Reference to the GameManager script
    void Awake(){
        // Find GameManager in the scene
        gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager == null) Debug.LogError("GameManager not found in the scene!");
    }
    private void OnEnable(){
        // Add listener to the Pause button
        if (PauseButton != null) PauseButton.onClick.AddListener(GamePause);
    }
    private void OnDisable(){
        // Clean up button listeners
        if (PauseButton != null) PauseButton.onClick.RemoveListener(GamePause);
    }
    private void Start(){
        // Check if UI references are assigned
        if (PausePanel == null || PauseButton == null){
            Debug.LogError("ShopManager: UI references are not assigned!");
            return;
        }

        // Calculate positions dynamically based on the RectTransform's local coordinates
        CalculatePositions();

        // Hide the shop panel at the start
        PausePanel.anchoredPosition = offScreenPosition;
    }

    private void Update(){
        if (PausePanel == null) return;
        if(Input.GetKeyDown(KeyCode.Escape)){
            if (ifPause) GameResume();
            else GamePause();
        }
        // Smoothly move the pause panel
        if (ifPause){
            PausePanel.anchoredPosition = Vector2.MoveTowards(PausePanel.anchoredPosition, onScreenPosition, slideSpeed * Time.unscaledDeltaTime );
        }
        else{
            PausePanel.anchoredPosition = Vector2.MoveTowards(PausePanel.anchoredPosition, offScreenPosition, slideSpeed * Time.unscaledDeltaTime );
        }
    }

    // Calculate the on-screen and off-screen positions dynamically
    private void CalculatePositions(){
        if (PausePanel == null) return;
        // Get the height of the pause panel in local coordinates
        float panelHeight = PausePanel.rect.height;

        // Define off-screen position as completely off the top of the screen
        offScreenPosition = new Vector2(0, panelHeight);

        // Define on-screen position as aligned to the top edge of the screen
        onScreenPosition = new Vector2(0, 0);
    }
    

    // Opens the shop panel
    public void GamePause(){
        // Check if UI references are assigned
        if (PausePanel == null || PauseButton == null) return;

        Debug.Log($"{nameof(GamePause)} called");
        ifPause = true;
        PauseButton.gameObject.SetActive(false);

        // Pause the game
        Time.timeScale = 0;
    }

    // Closes the shop panel
    public void GameResume(){
        // Check if UI references are assigned
        if (PausePanel == null || PauseButton == null) return;

        Debug.Log($"{nameof(GameResume)} called");
        ifPause = false;
        PauseButton.gameObject.SetActive(true);
        
        // Resume the game
        Time.timeScale = 1;
    }

    public void quitMap()
    {
        Time.timeScale = 1f; // Unpause before leaving, Prevents Frozen UI in Next Scene
        //Full game reset if returning to menu
        if (gameManager != null) gameManager.ResetGame(); // Clear gameplay state
        SceneManager.LoadScene("MainMenu");
    }
}