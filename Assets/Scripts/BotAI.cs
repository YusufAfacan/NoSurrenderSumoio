using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScoreCounter;

public class BotAI : MonoBehaviour
{
    private Wrestler wrestler;
    public bool lastHitByPlayer;
    public string wrestlerName;

    public event EventHandler OnDie;
    public event EventHandler<OnDefeatedByPlayerEventArgs> OnDefeatedByPlayer;
    public class OnDefeatedByPlayerEventArgs : EventArgs
    {
        public float bodySize;
    }

    private void Awake()
    {
        wrestler = GetComponent<Wrestler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<Player>())
        {
            lastHitByPlayer = true;
        }
        else
        {
            lastHitByPlayer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (lastHitByPlayer)
            {
                OnDefeatedByPlayer?.Invoke(this, new OnDefeatedByPlayerEventArgs()
                {
                    bodySize = wrestler.bodySize,
                });

            }

            OnDie?.Invoke(this, EventArgs.Empty);

            gameObject.SetActive(false);
        }

        
    }
}
