using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Slots : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Slot Machine Components")]
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite[] symbols;
    [SerializeField] private Button spinButton;
    [SerializeField] private TMP_Text resultText;

    [Header("Game Settings")]
    [SerializeField] private int betAmount = 50;
    [SerializeField] private float spinSpeed = 0.1f;
    [SerializeField] private float reelStopDelay = 0.5f;
    [SerializeField] private Vector2 spinDurationRange = new Vector2(2f, 4f);

    private bool isSpinning = false;
    private Dictionary<Sprite, int> symbolMultipliers = new Dictionary<Sprite, int>();
//Sets up the slot machine and refreshes the UI--------------------------------------------------------------------
    private void Start()
    {
        // Ensure Economy is ready
        if (Economy.Instance == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Rest of your existing Start() code
        InitializeSlotMachine();
        spinButton.onClick.AddListener(StartSpinning);
        
        // Refresh money display
        Economy.Instance.RefreshUI(moneyText); // Add this if you have a money display
    }
//Initializes the slot machine by assigning multipliers to each symbol--------------------------------------------------------------------
    private void InitializeSlotMachine()
    {
        if (symbols.Length != 3)
        {
            Debug.LogError("Slot machine requires exactly 3 symbols!");
            return;
        }

        symbolMultipliers = new Dictionary<Sprite, int>
        {
            { symbols[0], 2 }, { symbols[1], 4 }, { symbols[2], 8 }
        };
    }
//Starts spinning when player click spin --------------------------------------------------------------------
    public void StartSpinning()
    {
        if (isSpinning) return;

        if (!Economy.Instance.CanAfford(betAmount))
        {
            resultText.text = "Not enough money!";
            return;
        }

        Economy.Instance.AddMoney(-betAmount);
        StartCoroutine(SpinRoutine());
    }
//This handles the spin cycle such as spin anamation, stop reels,and check resault--------------------------------------------------------------------
    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        spinButton.interactable = false;
        resultText.text = "Spinning...";

        float spinDuration = Random.Range(spinDurationRange.x, spinDurationRange.y);
        yield return SpinAnimation(spinDuration);
        yield return StopReelsWithDelay();

        CheckWinCondition();
        Economy.Instance.RefreshUI(moneyText);
        isSpinning = false;
        spinButton.interactable = true;
    }
//Spins slot reel by changing the image quickly--------------------------------------------------------------------
    private IEnumerator SpinAnimation(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            for (int i = 0; i < slotImages.Length; i++)
            {
                slotImages[i].sprite = GetRandomSymbol();
            }
            elapsed += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }
    }
//Stops each reel one by one with a small delay--------------------------------------------------------------------
    private IEnumerator StopReelsWithDelay()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            yield return new WaitForSeconds(reelStopDelay);
            slotImages[i].sprite = GetRandomSymbol();
        }
    }

    private Sprite GetRandomSymbol() => symbols[Random.Range(0, symbols.Length)];
//Checks if the player has won --------------------------------------------------------------------
    private void CheckWinCondition()
    {
        if (IsJackpot())
        {
            AwardJackpot();
        }
        else
        {
            resultText.text = "Try again!";
        }
    }
//--------------------------------------------------------------------
    private bool IsJackpot() => 
        slotImages[0].sprite == slotImages[1].sprite && 
        slotImages[1].sprite == slotImages[2].sprite;
//Awards the player with the gold if they win --------------------------------------------------------------------
    private void AwardJackpot()
    {
        Sprite winningSymbol = slotImages[0].sprite;
        int multiplier = symbolMultipliers[winningSymbol];
        int winnings = betAmount * multiplier;

        Economy.Instance.AddMoney(winnings);
        Economy.Instance.RefreshUI(moneyText);
        resultText.text = $"Jackpot! You win {winnings} coins!";
    }
//Makes sure that the bet amount and speeds are correct.--------------------------------------------------------------------
    private void OnValidate()
    {
        betAmount = Mathf.Max(1, betAmount);
        spinSpeed = Mathf.Clamp(spinSpeed, 0.01f, 0.5f);
        reelStopDelay = Mathf.Clamp(reelStopDelay, 0.1f, 1f);
    }
//--------------------------------------------------------------------

//This button sends you back to the main menu --------------------------------------------------------------------
    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
//--------------------------------------------------------------------
}