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

    public BotAI lastHitBy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cameraControl = CameraControl.Instance;
        wrestlerCounter = WrestlerCounter.Instance;
        
        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
    }

    private void WrestlerCounter_OnNoBotRemaining(object sender, EventArgs e)
    {
        transform.DOLookAt(cameraControl.WinCam.transform.position, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthKit>() != null)
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
