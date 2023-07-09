using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotMovement : MonoBehaviour
{
    private bool isGoingForPepper;

    private WrestlerAnimator animator;
    private Wrestler wrestler;
    private Rigidbody rb;
    public GameObject movementFX;
    [SerializeField] private float sightRadius;

    private Timer timer;

    [SerializeField]
    [Range(0f, 10f)]
    private float movementSpeed;
    [SerializeField]
    [Range(360, 720)]
    private float rotationSpeed;

    private bool isGoingToTarget;

    public GameObject currentTarget;

    private bool hasTarget;

    private ObjectPooler pooler;
    private void Awake()
    {
        wrestler = GetComponent<Wrestler>();
        animator = GetComponent<WrestlerAnimator>();
        rb = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {
        pooler = ObjectPooler.Instance;
        timer = Timer.Instance;
        timer.OnTimeUp += Timer_OnTimeUp;
        pooler.OnObjectSpawned += Pooler_OnObjectSpawned;
        //StartCoroutine(DecideTarget());
        DecideNewTarget();
    }

    private void Pooler_OnObjectSpawned(object sender, EventArgs e)
    {
        DecideNewTarget();
    }

    private void Timer_OnTimeUp(object sender, EventArgs e)
    {
        StopMoving();
    }

    private void StopMoving()
    {
        wrestler.canMove = false;
    }

    private void DecideNewTarget()
    {
        if (wrestler.isOnFever)
        {
            currentTarget = ClosestWrestler();
        }
        else
        {
            int i = Random.Range(1, 101);

            if (50 < i) // 50 percent chance goes to item primarily pepper
            {
                if (ClosestPepper() != null)
                {
                    if(Vector3.Distance(ClosestPepper().transform.position, transform.position) <= sightRadius)
                    {
                        currentTarget = ClosestPepper();
                    }
                    

                }
                else if (ClosestHealthKit() != null)
                {
                    if (Vector3.Distance(ClosestHealthKit().transform.position, transform.position) <= sightRadius)
                    {
                        currentTarget = ClosestHealthKit();
                    }

                }
                else
                {
                    currentTarget = ClosestWrestler();

                }
            }
            if (21 < i && i <= 50) // 30 percent chance goes to closest wrestler
            {
                currentTarget = ClosestWrestler();
            }
            if (0 < i && i <= 20) // 20 percent chance goes to wrestler closest to water
            {
                currentTarget = WrestlerInWorstSituation();
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        DecideNewTarget();

    }

    private void OnTriggerEnter(Collider other)
    {
        DecideNewTarget();
    }


    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!wrestler.canMove)
        {
            animator.NotRunning();

            if (movementFX.activeSelf)
            {
                movementFX.SetActive(false);
            }

            return;
        }

        if (currentTarget != null && currentTarget.activeSelf)
        {
            Vector3 pos = transform.position;
            Vector3 toVector = currentTarget.transform.position - pos;
            toVector.Normalize();

            Vector3 toPosition = pos + wrestler.moveSpeed * Time.fixedDeltaTime * toVector;
            Quaternion toRotation = Quaternion.LookRotation(rotationSpeed * Time.fixedDeltaTime * toVector, Vector3.up);

            rb.Move(toPosition, toRotation);
            animator.Running();

            if (!movementFX.activeSelf)
            {
                movementFX.SetActive(true);
            }
        }
        else
        {
            DecideNewTarget();
        }
    }

    private GameObject ClosestHealthKit()
    {
        GameObject[] healthkits = GameObject.FindGameObjectsWithTag("HealthKit");

        if (healthkits.Length <= 0)
        {
            return null;
        }

        GameObject closestHealthKit = null;

        float distance = Mathf.Infinity;

        foreach (GameObject healthkit in healthkits)
        {
            if (Vector3.Distance(transform.position, healthkit.transform.position) <= distance)
            {
                distance = Vector3.Distance(transform.position, healthkit.transform.position);
                closestHealthKit = healthkit;
            }

        }

        return closestHealthKit;

    }

    private GameObject ClosestPepper()
    {
        GameObject[] peppers = GameObject.FindGameObjectsWithTag("Pepper");

        if (peppers.Length <= 0)
        {
            return null;
        }

        GameObject closestPepper = null;

        float distance = Mathf.Infinity;

        foreach (GameObject pepper in peppers)
        {
            if (Vector3.Distance(transform.position, pepper.transform.position) <= distance)
            {
                distance = Vector3.Distance(transform.position, pepper.transform.position);
                closestPepper = pepper;
            }

        }

        return closestPepper;

    }

    private GameObject ClosestWrestler()
    {
        List<GameObject> wrestlers = FindObjectsOfType<Wrestler>().Select(w => w.gameObject).ToList();
        wrestlers.Remove(gameObject);

        if (wrestlers.Count <= 0)
        {
            return null;
        }

        GameObject closestWrestler = null;

        float distance = Mathf.Infinity;

        foreach (GameObject wrestler in wrestlers)
        {
            if (Vector3.Distance(transform.position, wrestler.transform.position) <= distance)
            {
                distance = Vector3.Distance(transform.position, wrestler.transform.position);
                closestWrestler = wrestler;
            }

        }

        wrestlers.Clear();

        return closestWrestler;

    }

    private GameObject WrestlerInWorstSituation()
    {
        List<GameObject> wrestlers = FindObjectsOfType<Wrestler>().Select(w => w.gameObject).ToList();
        wrestlers.Remove(gameObject);

        if (wrestlers.Count <= 0)
        {
            return null;
        }

        GameObject worstWrestler = null;

        float distance = 0;

        foreach (GameObject wrestler in wrestlers)
        {

            // wrestler that is farthest to center and also closest to water detected 
            if (Vector3.Distance(Vector3.zero, wrestler.transform.position) >= distance)
            {
                distance = Vector3.Distance(Vector3.zero, wrestler.transform.position);
                worstWrestler = wrestler;
            }

        }

        return worstWrestler;


    }


}
