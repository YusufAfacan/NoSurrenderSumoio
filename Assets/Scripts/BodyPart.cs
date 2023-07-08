using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private Rigidbody rb;
    private Wrestler wrestler;

    public enum Type { Rear, Front, Side }
    public Type type;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        wrestler = GetComponentInParent<Wrestler>();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // check if this collided a wrestlers bodypart or scene prop
    //    if (collision.transform.TryGetComponent(out Wrestler collidedWrestler))
    //    {
    //        Rigidbody colliderWrestlerRb = collidedWrestler.GetComponent<Rigidbody>(); 
    //        Vector3 forceDirection = collidedWrestler.transform.position - transform.position; // calculate direction of force to be applied
    //        forceDirection.Normalize(); // vector normalized so we have only direction and not length
            

    //        if (type == Type.Front)
    //        {
    //            // force applied to this mitigated by bodySize
    //            rb.AddForce(wrestler.baseTakenForce / wrestler.bodySize * -forceDirection, ForceMode.Impulse);

    //            if()
    //        }

    //        if(type == Type.Side || type == Type.Rear)
    //        {
    //            // force applied to enemy amplified by bodySize
    //            colliderWrestlerRb.AddForce(wrestler.baseApplyForce * wrestler.bodySize * forceDirection, ForceMode.Impulse); 

    //        }
            


    //        //else // then this is bounced off of an another wrestler
    //        //{
    //        //    Vector3 forceDirection = transform.position - colliderWrestler.transform.position; // as we take force it is opposite above
    //        //    forceDirection.Normalize(); // vector normalized so we have only direction and not length
    //        //    rb.AddForce(baseTakenForce / bodySize * forceDirection); // force applied to this mitigated by bodySize
    //        //}
    //    }
    //}
}
