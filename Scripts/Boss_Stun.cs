using UnityEngine;

public class Boss_Stun : StateMachineBehaviour
{
    public float stunDuration = 4f;
    private float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = stunDuration;

        Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("IsEnraged", true);

        BossHealth health = animator.GetComponent<BossHealth>();
        if (health != null)
        {
            health.isInvulnerable = true;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            animator.SetTrigger("Enrage");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Enrage");

        BossHealth health = animator.GetComponent<BossHealth>();
        if (health != null)
        {
            health.isInvulnerable = false;
        }
    }
}
