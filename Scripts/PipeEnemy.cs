using UnityEngine;

public class PipeEnemy : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float maxHeight = 1.6f;
    private Vector3 startPos;

    [Header("Detection")]
    public float minDistance = 3f;
    public float maxDistance = 10f;

    [Header("Shooting")]
    public EnemyBullet.BulletType bulletType;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private float nextFireTime;

    private float activationDelay = 0.5f;
    private float activationTime;

    private enum State { Hidden, Rising, Active, Lowering }
    private State currentState = State.Hidden;

    private Vector3 originalScale;

    void Start()
    {
        startPos = transform.position;
        originalScale = transform.localScale;
    }

    void Update()
    {
        Flip();

        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Hidden:
                if (distance > minDistance && distance < maxDistance)
                {
                    currentState = State.Rising;
                }
                break;

            case State.Rising:
                if (distance <= minDistance)
                {
                    currentState = State.Lowering;
                    return;
                }

                MoveUp();

                if (transform.position.y >= startPos.y + maxHeight) 
                { 
                    activationTime = Time.time;
                    currentState = State.Active;
                }
                break;

            case State.Active:
                if (distance <= minDistance || distance >= maxDistance)
                {
                    currentState = State.Lowering;
                    return;
                }

                if (Time.time > activationTime + activationDelay)
                {
                    Shoot();
                }
                break;

            case State.Lowering:
                MoveDown();
                break;
        }
    }

    void MoveUp()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }

    void MoveDown()
    {
        transform.position -= Vector3.up * moveSpeed * Time.deltaTime;

        if (transform.position.y <= startPos.y)
        {
            transform.position = startPos;
            currentState = State.Hidden;
        }
    }

    void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Vector2 dir = (player.position - firePoint.position).normalized;

            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            bulletScript.SetDirection(dir);
            bulletScript.bulletType = bulletType;

            nextFireTime = Time.time + fireRate;
        }

        Debug.DrawLine(firePoint.position, player.position, Color.red, 1f);
    }

    void Flip()
    {
        if (player == null) return;

        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
}
