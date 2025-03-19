using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // [SerializeField] is an attribute in Unity that allows you to keep private variables visible and editable in the Inspector while maintaining encapsulation in your code.
    [SerializeField] private RectTransform shopPanel;  // Reference to the Shop Panel
    [SerializeField] private Button startButton;       // Reference to the Start Button
    [SerializeField] private float slideSpeed = 500f;  // Speed of the slide animation

    private Vector2 offScreenPosition;
    private Vector2 onScreenPosition;
    private bool isShopOpen = false;

    private void Start(){
        // Check if UI references are assigned
        if (shopPanel == null || startButton == null){
            Debug.LogError("ShopManager: UI references are not assigned!");
            return;
        }

        // Calculate positions dynamically based on the RectTransform's local coordinates
        CalculatePositions();

        // Hide the shop panel at the start
        shopPanel.anchoredPosition = offScreenPosition;
    }

    private void Update(){
        // Smoothly move the shop panel
        if (isShopOpen){
            shopPanel.anchoredPosition = Vector2.MoveTowards(shopPanel.anchoredPosition, onScreenPosition, slideSpeed * Time.deltaTime);
        }
        else{
            shopPanel.anchoredPosition = Vector2.MoveTowards(shopPanel.anchoredPosition, offScreenPosition, slideSpeed * Time.deltaTime);
        }
    }

    // Calculate the on-screen and off-screen positions dynamically
    private void CalculatePositions(){
        // Get the width of the shop panel in local coordinates
        float panelWidth = shopPanel.rect.width;

        // Define off-screen position as completely off the right side of the screen
        offScreenPosition = new Vector2(panelWidth, 0);

        // Define on-screen position as aligned to the right edge of the screen
        //Reasin This is on the right edge is because I set the anchor in inspector of the shopPanel in Unity to be middle right
        onScreenPosition = new Vector2(0, 0);
    }

    // Opens the shop panel
    public void OpenShop(){
        // Check if UI references are assigned
        if (shopPanel == null || startButton == null) return;

        Debug.Log($"{nameof(OpenShop)} called");
        isShopOpen = true;
        startButton.gameObject.SetActive(false);
    }

    // Closes the shop panel
    public void CloseShop(){
        // Check if UI references are assigned
        if (shopPanel == null || startButton == null) return;

        Debug.Log($"{nameof(CloseShop)} called");
        isShopOpen = false;
        startButton.gameObject.SetActive(true);
    }
}