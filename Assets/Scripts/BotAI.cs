using System;
using UnityEngine;

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
