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

    private WrestlerCounter wrestlerCounter;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        timeRemaining = gameTime;
        wrestlerCounter = WrestlerCounter.Instance;

        InvokeRepeating(nameof(CountDown), 1, 1);
        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
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
            TimeIsUp();
        }
    }

    private void TimeIsUp()
    {
        
    }
}
