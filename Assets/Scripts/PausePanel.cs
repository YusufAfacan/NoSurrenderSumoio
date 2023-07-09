using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Button resumeButton;
    public GameObject pausePanel;

    void Start()
    {
        resumeButton.onClick.AddListener(() => ResumeGame());
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

}
