using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Stun : StateMachineBehaviour
{
    public float stunDuration = 2f;
    private float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = stunDuration;
        Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;  //Pysäytä liike
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
    }
}
