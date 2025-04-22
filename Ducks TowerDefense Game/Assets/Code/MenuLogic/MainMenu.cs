using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    public GameObject ShopUI;

    void Start()
    {
        ShopUI.SetActive(false); // Hide the shop UI at the start
    }
    void Update()
    {
        // Reset money when D is pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            Economy.Instance.AddMoney(-Economy.Instance.Money); // Set to 0
            Economy.Instance.AddMoney(100); // Reset to default
        }
    }

    //When you come back to the main menu from another scene, this ensures your money display (moneyText) shows the current amount
    //OnEnable() runs every time the GameObject becomes active (including when returning to the menu scene)
    void OnEnable()
    {
        if (Economy.Instance != null)
        {
            Economy.Instance.RefreshUI(moneyText);
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene("Map Selection");
    }

    public void PlayMiniGameSlots() {
        SceneManager.LoadScene("SlotMachine", LoadSceneMode.Single);
    }

    public void Options() {

    }

    public void QuitGame() {
        Application.Quit();
    }
    public void OpenShop() {
        ShopUI.SetActive(true); // Show the shop UI
        Time.timeScale = 0; // Pause the game
    }
    public void CloseShop() {
        ShopUI.SetActive(false); // Hide the shop UI
        Time.timeScale = 1; // Resume the game
    }
}