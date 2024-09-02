using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;  // Objektien liikkumisnopeudeksi on m‰‰ritetty 1f.
    public Vector2 direction = Vector2.left;  // Objektit liikkuvat vasemmalle.
    public float gravity = -9.81f;  // Painovoiman voi m‰‰ritt‰‰ myˆs -> Unity -> Edit -> Project Settings -> Physics 2D -> Gravity
                                    // T‰‰ll‰ voi muuttaa tˆrm‰ysasetuksia, jotta esim. viholliset eiv‰t tˆrm‰‰ toisiinsa: Physics 2D -> Layer Collission Matrix -> Ota Enemyn alin t‰pp‰ pois.
    SpriteRenderer SR;
    private new Rigidbody2D rigidbody;  // Lis‰‰m‰ll‰ sanan "new" rigidbodyn eteen, saa poistettua virheilmoituksen.
    private Vector2 velocity;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;  // Objektien ei haluta liikkuvan heti, vaan ainoastaan sitten kun ne aktivoituvat.
    }

    private void OnBecameVisible()  // Objektit aktivoituvat kun ne n‰kyv‰t n‰ytˆll‰.
    {
        enabled = true;
    }

    private void OnBecameInvisible()  // Objektit eiv‰t ole aktiivisia kun ne eiv‰t n‰y n‰ytˆll‰.
    {
        enabled = false;
    }

    private void OnEnable()  // Objekti liikkuu kun se on aktivoitu.
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()  // Objekti ei liiku kun se ei ole aktiivinen.
    {
        rigidbody.velocity = Vector2.zero;  // Objektin liike pys‰ytet‰‰n.
        rigidbody.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if (rigidbody.Raycast(direction))  // Jos objekti osuu johonkin...
        {
            direction = -direction;  //...se muuttaa suuntaa. Raycast on aiemmin m‰‰ritelty Extensions -laajennus skirtsiss‰.
            SR.flipX = !SR.flipX;  //...k‰‰nnet‰‰n renderer kulkusuuntaan.
        }
        
        if (rigidbody.Raycast(Vector2.down))  // Jos objekti on maassa...
        {
            velocity.y = Mathf.Max(velocity.y, 0f);  // ...sovelletaan matematiikan f-luokan nopeutta. Kun objekti on ilmassa painovoima vaikuttaa siihen ja t‰t‰ ei huomioida.
        }
    }
}
