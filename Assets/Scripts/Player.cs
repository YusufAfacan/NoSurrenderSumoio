using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour
{
    public static Player Instance;

    public event EventHandler OnHealthKitPicked;
    public event EventHandler OnDie;

    private WrestlerCounter wrestlerCounter;
    private CameraControl cameraControl;
    private Timer timer;

    [HideInInspector] public BotAI lastHitBy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cameraControl = CameraControl.Instance;
        wrestlerCounter = WrestlerCounter.Instance;
        timer = Timer.Instance;
        
        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
        timer.OnTimeUp += Timer_OnTimeUp;
    }

    private void Timer_OnTimeUp(object sender, EventArgs e)
    {
        LookAtCam();
    }

    private void WrestlerCounter_OnNoBotRemaining(object sender, EventArgs e)
    {
        LookAtCam();
    }

    private void LookAtCam()
    {
        transform.DORotate(new Vector3(0f,180,0f), 0.5f);
        //Vector3 lookAt = new Vector3(cameraControl.WinCam.transform.position.x, transform.position.y, cameraControl.WinCam.transform.position.z);
        //transform.DOLookAt(lookAt, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthKit"))
        {
            OnHealthKitPicked?.Invoke(this, EventArgs.Empty);
        }

        if (other.CompareTag("Water"))
        {
            OnDie?.Invoke(this, EventArgs.Empty);

            gameObject.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<BotAI>())
        {
            lastHitBy = collision.transform.GetComponent<BotAI>();
        }
    }
}
