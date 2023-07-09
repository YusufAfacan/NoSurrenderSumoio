using Cinemachine;
using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

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

        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
    }

    private void WrestlerCounter_OnNoBotRemaining(object sender, EventArgs e)
    {
        topDownCam.gameObject.SetActive(false);
        WinCam.gameObject.SetActive(true);
    }
}
