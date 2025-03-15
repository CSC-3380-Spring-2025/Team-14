using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour{
    //[SerializeField] is an attribute in Unity that allows you to keep private variables visible and editable in the Inspector while maintaining encapsulation in your code.
    [SerializeField] private RectTransform shopPanel;  // Reference to the Shop Panel
    [SerializeField] private Button startButton;       // Reference to the Start Button

    private Vector2 offScreenPosition;
    private Vector2 onScreenPosition;

    private void Start(){
        if (shopPanel == null || startButton == null){
            Debug.LogError("ShopManager: UI references are not assigned!");
            return;
        }

        // Define positions based on the panel width
        offScreenPosition = new Vector2(292 * 2, 0); 
        onScreenPosition = new Vector2(292, 0);

        // Hide the shop panel at the start
        shopPanel.anchoredPosition = offScreenPosition;
    }

    // Opens the shop panel
    public void OpenShop(){
        if (shopPanel == null || startButton == null) return;

        Debug.Log($"{nameof(OpenShop)} called");
        shopPanel.anchoredPosition = onScreenPosition;
        startButton.gameObject.SetActive(false);
    }

    // Closes the shop panel
    public void CloseShop(){
        if (shopPanel == null || startButton == null) return;

        Debug.Log($"{nameof(CloseShop)} called");
        shopPanel.anchoredPosition = offScreenPosition;
        startButton.gameObject.SetActive(true);
    }
}
