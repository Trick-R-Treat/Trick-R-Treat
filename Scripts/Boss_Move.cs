using UnityEngine;

public class Boss_Move : StateMachineBehaviour
{
    public float normalSpeed = 2f;
    public float enragedSpeed = 5f;
    public float attackRange = 3f;

    public float jumpChance = 0.8f;

    Transform player;
    Rigidbody2D rb;
    Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();

        if (rb == null || !rb.simulated)
            return;

        if (boss != null && boss.GetComponent<BossHealth>().IsDead)
            return;

        boss.LookAtPlayer();

        bool isEnraged = animator.GetBool("IsEnraged");
        //Debug.Log("Animator IsEnraged: " + animator.GetBool("IsEnraged"));
        
        float speed = isEnraged ? enragedSpeed : normalSpeed;
        //Debug.Log("IsEnraged: " + isEnraged + " Speed: " + speed);

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

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Jump");
    }
}
