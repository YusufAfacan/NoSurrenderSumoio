using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Button pauseButton;
    public GameObject pausePanel;
    private void Start()
    {
        pauseButton.onClick.AddListener(() => PauseGame());
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
}
