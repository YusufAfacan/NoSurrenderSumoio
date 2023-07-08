using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrestler : MonoBehaviour
{
    public bool canMove;
    public int bodySize; // size of body effects both physical appearance regardig transform scale and forces to be applied
    public float forceAppliedWithFront; // base amount of force applied to opponent when intentionally ramming
    public float backfireForce; // base amount of force applied to self when intentionally ramming
    public float headToHeadBackFireForce; // base amount force applied both wrestler ramming head to head
    public float bouncedOffForce; // base amount of force applied to self when unintentionally ramming

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        
    }

    private void OnCollisionEnter(Collision Collision)
    {
        // check if this collided with an opponent wrestler or scene prop
        if (Collision.transform.TryGetComponent(out Wrestler collidedWrestler))
        {
            Vector3 pos = transform.position;
            Vector3 collidedWrestlerPos = collidedWrestler.transform.position;
            Vector3 forceDirection = collidedWrestlerPos - pos;
            forceDirection.Normalize();
            // get rigidbody of opponent to apply force later on
            Rigidbody colliderWrestlerRb = collidedWrestler.GetComponent<Rigidbody>(); 
            // get where collision happened
            ContactPoint firstContactPoint = Collision.GetContact(0);
            // get position where collision happened
            Vector3 firstContactPointPos = new Vector3(firstContactPoint.point.x, firstContactPoint.point.y, firstContactPoint.point.z);
            // caching which direction both this and collidedWrestler looking at
            Vector3 lookingDir = transform.forward;
            Vector3 enemyLookingDir = collidedWrestler.transform.forward;

            //angles between where collision happened and looking direction is used to determine if collision happened
            // in front of wrestler or rear
            float selfAngle = Vector3.Angle(firstContactPointPos - transform.position, lookingDir);
            float enemyAngle = Vector3.Angle(firstContactPointPos - collidedWrestler.transform.position, enemyLookingDir);

            // 60 degree to left and 60 degree to right of looking direction considered front
            // remaining 240 degree of 360 degree is considered back
            if (selfAngle <= 60f)
            {
                Debug.Log(transform.name + " hitting with front");

                if (enemyAngle >= 120f)
                {
                    Debug.Log("to the behind of " + collidedWrestler.transform.name);

                    colliderWrestlerRb.AddForce(forceAppliedWithFront * bodySize * forceDirection, ForceMode.Impulse); // force applied to enemy amplified by bodySize
                    rb.AddForce(backfireForce / bodySize * -forceDirection, ForceMode.Impulse); // force applied to this mitigated by bodySize

                }

                if (enemyAngle < 120f)
                {
                    Debug.Log("to the front of " + collidedWrestler.transform.name);

                    colliderWrestlerRb.AddForce(forceAppliedWithFront * bodySize * forceDirection, ForceMode.Impulse); // force applied to enemy amplified by bodySize
                    rb.AddForce(headToHeadBackFireForce / bodySize * -forceDirection, ForceMode.Impulse); // force applied to this mitigated by bodySize
                }

            }
            if (selfAngle > 60f)
            {
                Debug.Log(transform.name + " hitting with back to " + collidedWrestler.transform.name);

                rb.AddForce(bouncedOffForce / bodySize * -forceDirection, ForceMode.Impulse); // force applied to this mitigated by bodySize
            }
        }
    }
}


