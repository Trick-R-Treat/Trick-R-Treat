using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;

    public Vector2 direction = Vector2.left;

    //Gravity can also be determined > Unity > Edit > Project Settings > Physics 2D > Gravity
    public float gravity = -9.81f;

    SpriteRenderer SR;
    
    private new Rigidbody2D rigidbody;
    
    private Vector2 velocity;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
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
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if (rigidbody.Raycast(direction))
        {
            direction = -direction;
            SR.flipX = !SR.flipX;
        }

        if (rigidbody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }
}
