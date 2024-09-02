using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    
    private Vector2 velocity;
    private float inputAxis;
    
    public float moveSpeed = 8f;  // Luodaan Inspectoriin oma osio, jonka avulla pystyy s‰‰t‰m‰‰n helposti pelaajan liikkumisnopeutta.
    public float maxJumpHeight = 5f;  // M‰‰ritet‰‰n hyppykorkeus. T‰ss‰ se on 5 yksikkˆ‰.
    public float maxJumpTime = 1f;  // M‰‰ritet‰‰n hyppyaika. T‰ss‰ se on 1 sekuntti.
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);  // M‰‰ritet‰‰n hyppyvoima. Mit‰ kauemmin pelaaja pit‰‰ napin pohjassa, sit‰ korkeammalle/pidemm‰lle pelaaja hypp‰‰.
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);  // M‰‰ritet‰‰n painovoima, joka vet‰‰ pelaajan hypyn j‰lkeen alas.

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);


    private void Awake()  // Pyydet‰‰n Unitya etsim‰‰n komponentti.
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        camera = Camera.main;
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()  // T‰t‰ toimintoa kutsutaan jokaisessa pelin k‰ynniss‰ olevassa ruudussa.
    {
        HorizontalMovement();  // Vaakasuoraan liikkuminen.
        
        grounded = rigidbody.Raycast(Vector2.down);  // Tarkastaa onko pelaaja maassa.

        if (grounded)   // Jos pelaaja on maassa...
        {
            GroundedMovement();  //...toteutetaan GroundedMovement.
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);  // Rajataan kameran vasen reuna, jotta pelaaja ei voi liikkua sen ulkopuolelle. Zero on riippumaton n‰ytˆn kuvakoosta.
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f); // M‰‰ritet‰‰n, ett‰ pelaaja pysyy mini- ja maksimiarvojen sis‰ll‰.
                                                                                         // Koska pelaajan keskipiste on sen keskell‰, niin luvuilla +0.5 ja -0.5 m‰‰ritet‰‰n, ett‰ pelaaja n‰kyy kokonaan ruudussa, eik‰ katoa puoliksi sen ulkopuolelle.

        rigidbody.MovePosition(position);
        }


    private void HorizontalMovement()  // Pelaajan vaakasuoraan liikkuminen.
    {
        inputAxis = Input.GetAxis("Horizontal");  // M‰‰ritet‰‰n liikkumisen akseli: vaakataso.
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime); // T‰ss‰ asetetaan pelaajan nopeus siten, ett‰ kiihtyvyys ja hidastuvuus on n‰ht‰viss‰. Mathf on funktio ja tarkoittaa matematiikan f-luokkaa.

        if (rigidbody.Raycast(Vector2.right * velocity.x))  // Jos pelaaja tˆrm‰‰ esim. putkeen, vauhti pys‰htyy. Ilman t‰t‰ pelaaja n‰ytt‰isi hetkellisesti liimautuvan objektiin kiinni ja vasta sitten se kykenisi vaihtamaan suuntaa.
        {
            velocity.x = 0f;
        }
        
        if (velocity.x > 0f)  // Jos nopeus on suurempi kuin nolla, pelaajan kierto k‰‰ntyy oikealle.
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)  // Jos nopeus on pienempi kuin nolla, pelaajan kierto k‰‰ntyy vasemmalle. Jos 180f sijaan olisi 0f, pelaaja pyˆrisi paperikuvana paikallaan.
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;
        
        if (Input.GetButtonDown("Jump"))  // Jos hyppy-nappi on painettu...
        {
            velocity.y = jumpForce;
            jumping = true;  //...pelaaja hypp‰‰.
        }
    }

    private void ApplyGravity()
    {
        //Tsekkaa jos putoaa
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;
        
        //Soveltaa painovoimaa ja p‰‰tenopeutta
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)  // M‰‰ritet‰‰n tˆrm‰sikˆ pelaaja johonkin. Inspectoriin on Layers -kohtaan lis‰tty "PowerUp ja Enemy".
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))  // Jos pelaaja tˆrm‰‰ vihollisen kanssa...
        {
            if (transform.DotTest(collision.transform, Vector2.down))  // Jos tˆrm‰ys tulee ylh‰‰lt‰ alas...
            {
                velocity.y = jumpForce / 2f;  //...pelaaja hypp‰‰ pois kohteen p‰‰lt‰.
                jumping = true;
            }
        }

        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))  // Jos pelaaja tˆrm‰‰ yll‰tysboxin kanssa...
        {
            if (transform.DotTest(collision.transform, Vector2.up))  // Jos pelaaja lyˆ p‰‰ns‰ objektiin...
            {
                velocity.y = 0f;  //...vauhti pys‰htyy ja pelaaja putoaa alas. Ilman t‰t‰ pelaaja n‰ytt‰isi hetkellisesti liimautuvan kiinni objektiin, jonka j‰lkeen se vasta putoaisi alas.
            }
        }
    }
}
