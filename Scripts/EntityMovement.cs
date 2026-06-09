using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;

    public Vector2 direction = Vector2.left;

    //Unity > Edit > Project Settings > Physics 2D > Gravity
    public float gravity = -9.81f;

    SpriteRenderer SR;

    private Rigidbody2D rb;

    private Vector2 velocity;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rb.WakeUp();
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        if (rb.Raycast(direction))
        {
            direction = -direction;
            SR.flipX = !SR.flipX;
        }

        if (rb.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }
}
