using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI moneyText;
    public GameObject ShopUI;

//Hides the shope UI when the main menu starts
    void Start() => ShopUI.SetActive(false); // Hide the shop UI at the start
    
//This is not needed this was just used for the live demo
    // void Update()
    // {
    //     // Reset money when D is pressed this is used for the live demo not needed in real game 
    //     if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         Economy.Instance.AddMoney(-Economy.Instance.Money); // Set to 0
    //         Economy.Instance.AddMoney(500); // Reset to default
    //     }
    // }

//When you come back to the main menu from another scene, this ensures your money display (moneyText) shows the current amount
//OnEnable() runs every time the GameObject becomes active (including when returning to the menu scene)
    void OnEnable(){
        if (Economy.Instance != null) Economy.Instance.RefreshUI(moneyText);
    }

//Loads the map selection screen
    public void PlayGame() {
        SceneManager.LoadScene("Map Selection");
    }

//loads the slot machine minigame
    public void PlayMiniGameSlots() => SceneManager.LoadScene("SlotMachine", LoadSceneMode.Single);
    
// Not needed for now but can be used for future options menu
    //public void Options() {

    //}

//quits the game 
    public void QuitGame() => Application.Quit();
    
//Opens the shop UI
    public void OpenShop() {
        ShopUI.SetActive(true); // Show the shop UI
        Time.timeScale = 0; // Pause the game
    }

//Closes the shop UI
    public void CloseShop() {
        ShopUI.SetActive(false); // Hide the shop UI
        Time.timeScale = 1; // Resume the game
    }
}//End of MainMenu.cs