using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Jump : StateMachineBehaviour
{

    public float jumpForce = 8f;

    Rigidbody2D rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Jos boss saavuttaa maan hypyn j‰lkeen, siirryt‰‰n Attack-tilaan
        if (rb.velocity.y <= 0 && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            animator.SetTrigger("Attack");
        }
    }

    private bool IsGrounded(Animator animator)
    {
        return Physics2D.Raycast(animator.transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Jump");
    }
}