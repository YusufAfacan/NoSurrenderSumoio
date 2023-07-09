using Cinemachine;
using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

    private Timer timer;
    public CinemachineVirtualCamera topDownCam;
    public CinemachineVirtualCamera WinCam;

    private WrestlerCounter wrestlerCounter;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
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
