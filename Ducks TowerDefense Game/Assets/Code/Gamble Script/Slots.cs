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

    private void Start()
    {
        if (Economy.Instance == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        InitializeSlotMachine();
        spinButton.onClick.AddListener(StartSpinning);
        
        // update money text to reflect current balance when scene loads
        Economy.Instance.RefreshUI(moneyText); 
    }

    private void InitializeSlotMachine()
    {
        // assigns payout multipliers
        symbolMultipliers = new Dictionary<Sprite, int>
        {
            { symbols[0], 2 }, 
            { symbols[1], 4 }, 
            { symbols[2], 8 }
        };
    }

    public void StartSpinning()
    {
        //allow spin if its not already spinning and enough money
        if (isSpinning) return;

        if (!Economy.Instance.CanAfford(betAmount))
        {
            resultText.text = "Not enough money!";
            return; // cant play with no money
        }

        Economy.Instance.AddMoney(-betAmount);
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine() // logic for the script for the slots 
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

    private IEnumerator SpinAnimation(float duration) // changes the  actual images in the slots
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

    private IEnumerator StopReelsWithDelay() // stops the images
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            yield return new WaitForSeconds(reelStopDelay);
            slotImages[i].sprite = GetRandomSymbol();
        }
    }

    private Sprite GetRandomSymbol() => symbols[Random.Range(0, symbols.Length)];

    private void CheckWinCondition() // obvious
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

    private bool IsJackpot() => // checks for jackpot
        slotImages[0].sprite == slotImages[1].sprite && 
        slotImages[1].sprite == slotImages[2].sprite;

    private void AwardJackpot() // reward system
    {
        Sprite winningSymbol = slotImages[0].sprite;
        int multiplier = symbolMultipliers[winningSymbol];
        int winnings = betAmount * multiplier;
        Economy.Instance.AddMoney(winnings);
        Economy.Instance.RefreshUI(moneyText);
        resultText.text = $"Jackpot! You win {winnings} coins!";
    }

    private void OnValidate()
    {
        betAmount = Mathf.Max(1, betAmount);
        spinSpeed = Mathf.Clamp(spinSpeed, 0.01f, 0.5f);
        reelStopDelay = Mathf.Clamp(reelStopDelay, 0.1f, 1f);
    }



    public void goToMainMenu() // mainmenu button
    {
        SceneManager.LoadScene("MainMenu");
    }
}