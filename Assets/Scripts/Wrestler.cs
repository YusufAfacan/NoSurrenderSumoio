using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Wrestler : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove;
    public float moveSpeed;

    [Header("Bodysize")]
    public float bodySize; // size of body effects both physical appearance regardig transform scale and forces to be applied and taken
    public float bodySizeIncrement; // increase in bodySize for each item pickup
    [SerializeField] private float maxBodySize;

    [Header("Force and force related")]
    public bool isOnFever; // wrestlers can be on fever by picking up correlated item 
    public float feverMultiplier; // force amplifier value in fever mode
    public float FrontToFrontApplied; // base amount of force applied to opponent when intentionally ramming
    public float FrontToFrontTaken; // base amount of force applied to self when intentionally ramming
    public float FrontToBehindApplied; // base amount force applied both wrestler ramming head to head
    public float FrontToBehindTaken; // base amount of force applied to self when unintentionally ramming
    public float bounceOffTaken; // base amount of force applied to self when unintentionally ramming

    [SerializeField] private GameObject feverFireFX;

    private ObjectPooler pooler;
    private Rigidbody rb;

    private Wrestler lastHitBy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        pooler = ObjectPooler.Instance;
    }

    private void OnCollisionEnter(Collision Collision)
    {
        // check if this collided with an opponent wrestler or something else
        if (Collision.transform.TryGetComponent(out Wrestler collidedWrestler))
        {
            Vector3 pos = transform.position;
            Vector3 collidedWrestlerPos = collidedWrestler.transform.position;

            //detection direction of force to be applied
            Vector3 forceDirection = collidedWrestlerPos - pos;
            forceDirection.Normalize();

            // get rigidbody of opponent to apply force later on
            Rigidbody colliderWrestlerRb = collidedWrestler.GetComponent<Rigidbody>();

            // caching which direction both this and collidedWrestler looking at
            Vector3 lookingDir = transform.forward;
            Vector3 enemyLookingDir = collidedWrestler.transform.forward;

            // get position where collision happened
            Vector3 firstContactPoint = (transform.position + collidedWrestler.transform.position) / 2;

            //angles between where collision happened and looking direction is used to determine if collision happened
            // in front or rear of wrestler
            float selfAngle = Vector3.Angle(firstContactPoint - transform.position, lookingDir);
            float enemyAngle = Vector3.Angle(firstContactPoint - collidedWrestler.transform.position, enemyLookingDir);

            // 60 degree to left and 60 degree to right of looking direction considered front
            // remaining 240 degree of 360 degree is considered back
            if (selfAngle <= 60f)
            {
                Debug.Log(transform.name + " hitting with front");

                if (enemyAngle >= 120f)
                {
                    if (isOnFever)
                    {
                        colliderWrestlerRb.AddForce(bodySize * feverMultiplier * FrontToBehindApplied * forceDirection, ForceMode.Impulse); 

                        pooler.SpawnFromObjectPooler("HitFX").transform.position = firstContactPoint;
                    }
                    else
                    {
                        colliderWrestlerRb.AddForce(bodySize * FrontToBehindApplied * forceDirection, ForceMode.Impulse);

                        rb.AddForce(FrontToBehindTaken / bodySize * -forceDirection, ForceMode.Impulse);
                    }

                    Debug.Log("to the behind of " + collidedWrestler.transform.name);

                }

                if (enemyAngle < 120f)
                {
                    if (isOnFever)
                    {
                        colliderWrestlerRb.AddForce(feverMultiplier * FrontToFrontApplied * bodySize * forceDirection, ForceMode.Impulse);
                    }
                    else
                    {
                        colliderWrestlerRb.AddForce(FrontToFrontApplied * bodySize * forceDirection, ForceMode.Impulse);
                        rb.AddForce(FrontToFrontTaken / bodySize * -forceDirection, ForceMode.Impulse);
                    }

                    Debug.Log("to the front of " + collidedWrestler.transform.name);
                }

            }

            if (selfAngle > 60f)
            {
                Debug.Log(transform.name + " hitting with back to " + collidedWrestler.transform.name);

                rb.AddForce(bounceOffTaken / bodySize * -forceDirection, ForceMode.Impulse);
            }

            lastHitBy = collidedWrestler;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthKit"))
        {
            IncreaseBodySize();
            other.gameObject.SetActive(false);

        }

        if (other.CompareTag("Pepper"))
        {
            if (!isOnFever)
            {
                StartCoroutine(GoFever());
                other.gameObject.SetActive(false);
            }

        }

        if (other.CompareTag("Water"))
        {
            if (lastHitBy == null)
            {
                return;
            }

            lastHitBy.IncreaseBodySize();
        }
    }

    private IEnumerator GoFever()
    {
        isOnFever = true;
        moveSpeed = 10f;
        feverFireFX.SetActive(true);
        yield return new WaitForSeconds(7);
        isOnFever = false;
        moveSpeed = 5f;
        feverFireFX.SetActive(false);

    }

    public void IncreaseBodySize()
    {
        bodySize += bodySizeIncrement;
        bodySize = Mathf.Clamp(bodySize, 1f, maxBodySize);

        transform.DOScale(bodySize * Vector3.one, 0.5f);
    }
}


