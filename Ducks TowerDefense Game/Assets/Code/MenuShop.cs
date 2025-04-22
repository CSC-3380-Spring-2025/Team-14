using UnityEngine;

public class MenuShop : MonoBehaviour
{
    public GameObject ShopUI;
    // Start is called before the first frame update
    void Start()
    {
        ShopUI.SetActive(false); // Hide the shop UI at the start
    }

    // Update is called once per frame
    void Update()
    {
        // Handle shop menu updates
    }

    // Add methods to handle shop interactions
    public void PurchaseItem(string itemName)
    {
        // Implement purchase logic
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
