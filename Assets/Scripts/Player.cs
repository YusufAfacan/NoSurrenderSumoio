using System;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public event EventHandler OnHealthKitPicked;
    public event EventHandler OnDie;

    private WrestlerCounter wrestlerCounter;
    private Timer timer;

    [HideInInspector] public Bot lastHitBy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthKit"))
        {
            OnHealthKitPicked?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Arena"))
        {
            OnDie?.Invoke(this, EventArgs.Empty);

            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Bot>())
        {
            lastHitBy = collision.transform.GetComponent<Bot>();
        }
    }
}
