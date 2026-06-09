using UnityEngine;

public class Boss_Jump : StateMachineBehaviour
{
    public float jumpForce = 8f;

    Rigidbody2D rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rb.linearVelocity.y <= 0 && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            animator.SetTrigger("Attack");
        }
    }

    private bool IsGrounded(Animator animator)
    {
        return Physics2D.Raycast(
            animator.transform.position,
            Vector2.down,
            0.1f,
            LayerMask.GetMask("Ground")
        );
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Jump");
    }
}
