using Cinemachine;
using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

    private WrestlerCounter wrestlerCounter;
    private Timer timer;

    public CinemachineVirtualCamera topDownCam;
    public CinemachineVirtualCamera WinCam;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        wrestlerCounter = WrestlerCounter.Instance;
        timer = Timer.Instance;
        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
        timer.OnTimeUp += Timer_OnTimeUp;
    }

    private void Timer_OnTimeUp(object sender, EventArgs e)
    {
        topDownCam.gameObject.SetActive(false);
        WinCam.gameObject.SetActive(true);
    }

    private void WrestlerCounter_OnNoBotRemaining(object sender, EventArgs e)
    {
        topDownCam.gameObject.SetActive(false);
        WinCam.gameObject.SetActive(true);
    }
}
