using System;
using TMPro;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private Wrestler wrestler;
    public bool lastHitByPlayer;
    public string wrestlerName;
    public TextMeshPro nameText;

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

    private void Start()
    {
        nameText.text = wrestlerName;
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

    private void OnCollisionExit(Collision collision)
    {
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Arena"))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            
        }

        
    }
}
