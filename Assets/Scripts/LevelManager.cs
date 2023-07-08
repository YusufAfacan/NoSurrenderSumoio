using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    private WinPanel winPanel;
    private LosePanel losePanel;

    private void Start()
    {
        winPanel = WinPanel.Instance;
        losePanel = LosePanel.Instance;

        winPanel.OnResultShown += WinPanel_OnResultShown;
        losePanel.OnResultShown += LosePanel_OnResultShown;
    }

    private void LosePanel_OnResultShown(object sender, EventArgs e)
    {
        Invoke(nameof(RestartGame), 5f);
    }

    private void WinPanel_OnResultShown(object sender, EventArgs e)
    {
        Invoke(nameof(RestartGame), 5f);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
