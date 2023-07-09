using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUpPanel : MonoBehaviour
{
    public static TimeUpPanel Instance;

    private ScoreCounter scoreCounter;
    public TextMeshProUGUI scoreText;

    public event EventHandler OnResultShown;
    private Timer timer;

    public GameObject timUpPanel; 

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        timer = Timer.Instance;
        scoreCounter = ScoreCounter.Instance;

        timer.OnTimeUp += Timer_OnTimeUp;
    }

    private void Timer_OnTimeUp(object sender, EventArgs e)
    {
        ShowTimeUpPanel();
    }

    private void ShowTimeUpPanel()
    {
        timUpPanel.SetActive(true);
        scoreText.text = scoreCounter.currentScore.ToString();
        OnResultShown?.Invoke(this, EventArgs.Empty);
    }

}
