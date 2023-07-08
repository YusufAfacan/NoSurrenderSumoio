using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public static WinPanel Instance;
    public TextMeshProUGUI scoreText;
    private ScoreCounter pointCounter;
    private WrestlerCounter wrestlerCounter;
    public GameObject winPanel;

    public event EventHandler OnResultShown;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        pointCounter = ScoreCounter.Instance;
        wrestlerCounter = WrestlerCounter.Instance;

        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
    }

    private void WrestlerCounter_OnNoBotRemaining(object sender, System.EventArgs e)
    {
        ShowWinPanel();
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
        scoreText.text = pointCounter.currentScore.ToString();

        OnResultShown?.Invoke(this, EventArgs.Empty);
    }
}
