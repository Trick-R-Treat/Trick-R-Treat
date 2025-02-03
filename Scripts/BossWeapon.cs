using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int attackDamage = 10;
    public int enragedAttackDamage = 20;

    public Vector3 attackOffset;
    public float attackRange = 2f;
    public LayerMask attackMask;

    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {

        if (!IsGrounded() && !animator.GetBool("IsEnraged"))
            return; //Ei hyökkää ilmassa ellei ole Enraged

        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if (colInfo != null)
        {
            //colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

    public void EnragedAttack()
    { 

        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if (colInfo != null)
        {
            //colInfo.GetComponent<PlayerHealth>().TakeDamage(enrageAttackDamage);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; //Jos Boss on kuollut, se ei voi enää vahingoittaa pelaajaa
        
        if (collision.gameObject.CompareTag("Player"))  //Jos Boss törmää pelaajan kanssa...
        {
            Player player = collision.gameObject.GetComponent<Player>();
            
            if (player != null && !player.starpower || !player.magicpower)
            {
                player.Hit();  //...pelaaja saa osuman.
            }
        }
    }
}