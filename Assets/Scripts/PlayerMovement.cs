using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;
    private WrestlerAnimator animator;
    private Wrestler wrestler;
    private Rigidbody rb;
    private WrestlerCounter wrestlerCounter;
    public GameObject movementFX;
    private Timer timer;

    [SerializeField]
    [Range(0f, 10f)]
    private float movementSpeed;
    [SerializeField]
    [Range(360, 720)]
    private float rotationSpeed;

    private void Awake()
    {
        wrestler = GetComponent<Wrestler>();
        animator = GetComponent<WrestlerAnimator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        timer = Timer.Instance;
        wrestlerCounter = WrestlerCounter.Instance;
        wrestlerCounter.OnNoBotRemaining += WrestlerCounter_OnNoBotRemaining;
        timer.OnTimeUp += Timer_OnTimeUp;
    }

    private void Timer_OnTimeUp(object sender, EventArgs e)
    {
        StopMoving();
    }

    private void WrestlerCounter_OnNoBotRemaining(object sender, EventArgs e)
    {
        StopMoving();
    }

    private void StopMoving()
    {
        wrestler.canMove = false;
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

        Vector3 inputVector = new(joystick.Horizontal, 0, joystick.Vertical);

        if (inputVector.magnitude <= 0)
        {
            animator.NotRunning();

            if (movementFX.activeSelf)
            {
                movementFX.SetActive(false);
            }

            return;
        }

        inputVector = inputVector.normalized;

        Vector3 pos = transform.position;
        Vector3 toPosition = pos + wrestler.moveSpeed * Time.fixedDeltaTime * inputVector;
        Quaternion toRotation = Quaternion.LookRotation(rotationSpeed * Time.fixedDeltaTime * inputVector, Vector3.up);

        rb.Move(toPosition, toRotation);
        animator.Running();

        if (!movementFX.activeSelf)
        {
            movementFX.SetActive(true);
        }
    }
}
