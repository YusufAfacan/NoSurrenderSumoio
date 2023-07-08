using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 120;
    public TextMeshProUGUI timeText;
    private void Start()
    {
        InvokeRepeating(nameof(CountDown), 1, 1);

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
