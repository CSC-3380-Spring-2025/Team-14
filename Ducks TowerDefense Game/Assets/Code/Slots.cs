using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public Image[] slotImages; // UI images representing slot reels
    public Sprite[] symbols; // Only 3 different symbols
    public Button spinButton;
    public Text resultText;
    public Economy economy; // Reference to Economy script

    private bool isSpinning = false;
    private float spinSpeed = 0.1f;
    private float reelStopDelay = 0.5f;

    public int betAmount = 50; // Cost per spin
    private Dictionary<Sprite, int> symbolMultipliers = new Dictionary<Sprite, int>();

    void Start()
    {
        spinButton.onClick.AddListener(StartSpinning);

        // Ensure there are exactly 3 symbols in the array
        if (symbols.Length != 3)
        {
            Debug.LogError("Error: There must be exactly 3 different symbols.");
            return;
        }

        // Assign multipliers (Example: Change values as needed)
        symbolMultipliers[symbols[0]] = 2;  // First Symbol (e.g., Kiwi)
        symbolMultipliers[symbols[1]] = 4;  // Second Symbol (e.g., Banana)
        symbolMultipliers[symbols[2]] = 8;  // Third Symbol (e.g., Apple)
    }

    public void StartSpinning()
    {
        if (!isSpinning && economy.money >= betAmount)
        {
            economy.AddMoney(-betAmount); // Deduct spin cost
            isSpinning = true;
            spinButton.interactable = false;
            StartCoroutine(SpinReels());
        }
        else if (economy.money < betAmount) // checks if we have a brokie lol
        {
            resultText.text = "Not enough money!"; 
        }
    }

    IEnumerator SpinReels()
    {
        float spinDuration = Random.Range(2f, 4f);
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            for (int i = 0; i < slotImages.Length; i++)
            {
                slotImages[i].sprite = symbols[Random.Range(0, symbols.Length)];
            }
            elapsed += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }

        // Stop each reel one at a time
        for (int i = 0; i < slotImages.Length; i++)
        {
            yield return new WaitForSeconds(reelStopDelay);
            slotImages[i].sprite = symbols[Random.Range(0, symbols.Length)];
        }

        isSpinning = false;
        spinButton.interactable = true;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (slotImages[0].sprite == slotImages[1].sprite && slotImages[1].sprite == slotImages[2].sprite)
        {
            Sprite winningSymbol = slotImages[0].sprite;
            int multiplier = symbolMultipliers[winningSymbol];

            int winnings = betAmount * multiplier;
            economy.AddMoney(winnings); // Add winnings to player's balance

            resultText.text = $"Jackpot! You win {winnings} coins!";
        }
        else
        {
            resultText.text = "Try again!";
        }
    }
}
