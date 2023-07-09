using System;
using TMPro;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    public static LosePanel Instance;

    private ScoreCounter scoreCounter;
    private WrestlerCounter wrestlerCounter;
    private Player player;

    public TextMeshProUGUI placementText;
    public TextMeshProUGUI pushedByText;
    public TextMeshProUGUI scoreText;
    public GameObject losePanel;

    public event EventHandler OnResultShown;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        scoreCounter = ScoreCounter.Instance;
        wrestlerCounter = WrestlerCounter.Instance;
        player = Player.Instance;

        player.OnDie += Player_OnDie;
    }

    private void Player_OnDie(object sender, System.EventArgs e)
    {
        ShowLosePanel();
    }

    private void ShowLosePanel()
    {
        losePanel.SetActive(true);
        placementText.text = "You're #" + (wrestlerCounter.aliveBotAmount + 1).ToString();

        if (player.lastHitBy != null)
        {
            pushedByText.text = "Pushed by " + player.lastHitBy.wrestlerName;
        }
        else
        {
            pushedByText.text = "You droppped";
        }
        
        scoreText.text = scoreCounter.currentScore.ToString();

        OnResultShown?.Invoke(this, EventArgs.Empty);
    }
}
