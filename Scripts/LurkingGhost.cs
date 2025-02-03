using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkingGhost : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveHeight = 2f;
    public float waitTime = 2f;

    [Header("Player Detection")]
    public Transform player;
    public float detectionRange = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingUp = true;
    private bool isWaiting = false;

    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * moveHeight;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerTooClose())
        {
        //Pelaaja liian lähellä, pysähdy ja näytä idle-animaatio
            if (animator) animator.SetBool("isIdle", true);
            return;
        }
        else
        {
            if (animator) animator.SetBool("isIdle", false);
        }
        
        if (!isWaiting)
        {
            MoveGhost();
        }
    }

    private void MoveGhost()
    {
        Vector3 target = movingUp ? targetPosition : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            StartCoroutine(WaitAtPosition());
        }
    }

    private System.Collections.IEnumerator WaitAtPosition()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        movingUp = !movingUp;
        isWaiting = false;
    }

    private bool PlayerTooClose()
    {
        if (player == null) return false;
        return Vector3.Distance(player.position, transform.position) < detectionRange;
    }
}