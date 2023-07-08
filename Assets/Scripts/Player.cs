using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BotAI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public event EventHandler OnHealthKitPicked;
    public event EventHandler OnDie;
    public BotAI lastHitBy;


    private void Awake()
    {
        Instance = this;
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
