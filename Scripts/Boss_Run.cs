using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float normalSpeed = 2f;
    public float enragedSpeed = 5f;
    public float attackRange = 3f;
    public float jumpChance = 0.8f;  //Normaalisti ei hypp‰‰

    Transform player;
    Rigidbody2D rb;
    Boss boss;
    
    // OnStateEnteri‰ kutsutaan, kun siirtym‰ alkaa ja tilakone alkaa arvioida t‰t‰ tilaa
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdatea kutsutaan jokaisessa p‰ivityskehyksess‰ OnStateEnter- ja OnStateExit-soittojen v‰lill‰
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();  //K‰‰nt‰‰ bossin katseen siihen suuntaan mihin se liikkuu

        bool isEnraged = animator.GetBool("IsEnraged");

        float speed = isEnraged ? enragedSpeed : normalSpeed;
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            if (Random.value < jumpChance)
            {
                animator.SetTrigger("Jump");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    // OnStateExit kutsutaan, kun siirtym‰ p‰‰ttyy ja tilakone lopettaa t‰m‰n tilan arvioinnin
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Jump");
    }
}
