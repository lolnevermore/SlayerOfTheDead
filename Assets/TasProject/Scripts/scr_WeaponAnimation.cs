using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_WeaponAnimation : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isWalking = animator.GetBool("forwardWalk");
        bool isRunning = animator.GetBool("forwardRun");

        bool forwardMovement = Input.GetKey("W");
        bool runMovement = Input.GetKey("Left Shift");

        if (!isWalking && forwardMovement)
        {
            animator.SetBool("forwardWalk", true);
        }

        if (isWalking && !forwardMovement)
        {
            animator.SetBool("forwardWalk", false);
        }

        if (!isRunning && (forwardMovement && runMovement))
        {
            animator.SetBool("forwardRun", true);
        }

        if (isRunning && (!forwardMovement || !runMovement))

        {
            animator.SetBool("forwardRun", false);
        }

    }
}
