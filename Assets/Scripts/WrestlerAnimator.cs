using UnityEngine;

public class WrestlerAnimator : MonoBehaviour
{
    Animator animator;

    private const string ISRUNNING = "isRunning";

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Running()
    {
        animator.SetBool( ISRUNNING, true );
    }

    public void NotRunning()
    {
        animator.SetBool( ISRUNNING, false );
    }
}
