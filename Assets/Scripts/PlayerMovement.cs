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

    [SerializeField]
    [Range(0f, 10f)]
    private float movementSpeed;
    [SerializeField]
    [Range(360, 720)]
    private float rotationSpeed;

    private Rigidbody rb;

    private void Awake()
    {
        wrestler = GetComponent<Wrestler>();
        animator = GetComponent<WrestlerAnimator>();
        rb = GetComponent<Rigidbody>();
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
            return;
        }

        Vector3 inputVector = new(joystick.Horizontal, 0, joystick.Vertical);

        if (inputVector.magnitude <= 0)
        {
            animator.NotRunning();
            return;
        }

        inputVector = inputVector.normalized;

        Vector3 pos = transform.position;
        Vector3 toPosition = pos + wrestler.moveSpeed * Time.deltaTime * inputVector;
        Quaternion toRotation = Quaternion.LookRotation(rotationSpeed * Time.fixedDeltaTime * inputVector, Vector3.up);

        rb.Move(toPosition, toRotation);
        animator.Running();

        
        
    }
}
