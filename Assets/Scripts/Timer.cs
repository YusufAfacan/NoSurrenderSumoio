using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public float gameTime = 120;
    public float timeRemaining;
    public TextMeshProUGUI timeText;

    public event EventHandler OnTimeUp;
    private Player player;
    private WrestlerCounter wrestlerCounter;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        timeRemaining = gameTime;
        wrestlerCounter = WrestlerCounter.Instance;
        player = Player.Instance;
        InvokeRepeating(nameof(CountDown), 1, 1);
        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
        player.OnDie += Player_OnDie;
    }

    private void Player_OnDie(object sender, EventArgs e)
    {
        CancelInvoke();
    }

    private void WrestlerCounter_OnNoBotRemaining(object sender, EventArgs e)
    {
        CancelInvoke();
    }

    private void CountDown()
    {
        timeRemaining--;
        timeText.text =timeRemaining.ToString();
        if (timeRemaining <= 0)
        {
            CancelInvoke();
            OnTimeUp?.Invoke(this, EventArgs.Empty);
        }
    }

}
