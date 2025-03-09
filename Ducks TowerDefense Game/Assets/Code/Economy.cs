using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour
{
    public Text moneyText;
    public int money = 500;

    void Start()
    {
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = money.ToString();
    }
}