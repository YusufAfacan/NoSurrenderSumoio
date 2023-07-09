using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WrestlerCounter : MonoBehaviour
{
    public static WrestlerCounter Instance;
    public Bot[] botsInGame;
    public int aliveBotAmount;
    public TextMeshProUGUI text;

    public event EventHandler OnNoBotRemaining;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        botsInGame = FindObjectsOfType<Bot>();

        foreach (Bot bot in botsInGame)
        {
            bot.OnDie += Bot_OnDie;
        }
        aliveBotAmount = botsInGame.Length;
        text.text = aliveBotAmount.ToString();
    }

    private void Bot_OnDie(object sender, EventArgs e)
    {
        CheckIfBotRemains();
    }

    private void CheckIfBotRemains()
    {
        aliveBotAmount--;

        text.text = aliveBotAmount.ToString();

        if (aliveBotAmount <= 0)
        {
            OnNoBotRemaining?.Invoke(this, new EventArgs());
        }
    }
}
