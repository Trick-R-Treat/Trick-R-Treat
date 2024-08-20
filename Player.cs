using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer; //Tätä tarvitaan seuraamaan kumpaa ylläolevista rendereistä milloinkin käytetään.

    private DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;  //Pienen hahmon collider on liian pieni isolle hahmolle, joten se pitää määrittää erikseen koodissa.

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower {  get; private set; }

    public bool magicpower { get; private set; }

    private DeathBarrier deathBarrier;

    //AUDIO
    public AudioClip jump;
    public AudioClip death;
    public AudioClip powerUp;
    public AudioClip shrink;
    public AudioClip starAudio;
    public AudioClip magicPowerAudio;
    private AudioSource audioSource;
    private AudioSource starAudioSource;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;

        starAudioSource = gameObject.AddComponent<AudioSource>();  //Erillinen audio komponentti tähtivoiman äänelle.
    }

    void Start()  //AUDIO
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()  //AUDIO
    {
        if (Input.GetButtonDown("Jump"))
        {
            PlaySound(jump);
        }
    }

    void PlaySound(AudioClip clip)  //AUDIO
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayStarSound(AudioClip clip)  //STAR AUDIO
    {
        starAudioSource.clip = clip;
        starAudioSource.Play();
    }

    public void Hit()  //Kun hahmo osuu viholliseen...
    {
        if (!dead && !starpower && !magicpower)  //Jos hahmo ei ole kuollut ja sillä ei ole tähtivoimaa...
        {
            if (big)  //Jos hahmo on iso...
            {
                Shrink();  //...se pienenee.
            }

            else  //Muuten...
            {
                Death();  //...se kuolee.
            }
        }
    }

    private void Death()
    {
        smallRenderer.enabled = false;  //Poistetaan pienen hahmon renderöinti käytöstä.
        bigRenderer.enabled = false;  //Poistetaan suuren hahmon renderöinti käytöstä.
        deathAnimation.enabled = true;  //Otetaan hahmon kuolema-animaatio käyttöön.
        PlaySound(death);

        GameManager.Instance.ResetLevel(3f);  //Nollataan taso 3 sekuntin kuluttua. Tähän on tärkeä määrittää aika, jotta kuolema-animaatiolla on aikaa rullata läpi.
    }

    public void Grow()
    {
        smallRenderer.enabled = false;  //Otetaan pienen hahmon renderer pois käytöstä.
        bigRenderer.enabled = true;  //Otetaan suuren hahmon renderer käyttöön.
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);  //Tässä kasvatetaan collider yhdestä yksiköstä kahteen.
        capsuleCollider.offset = new Vector2(0f, 0.5f);  //Tässä siirretään collideria puoli yksikköä ylöspäin, jotta se kohdistuu oikein.

        StartCoroutine(ScaleAnimation());
        PlaySound(powerUp);  //Soitetaan ääni kun hahmo kasvaa.
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
        PlaySound(shrink);
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)  //Kun on kulunut vähemmän kuin kesto jatkamme animointia...
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled= !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void Starpower(float duration = 10f)  //Tähtivoima kestää 10 sekunttia.
    {
        StartCoroutine(StarpowerCoroutine(duration));  //audio
        StartCoroutine(StarpowerAnimation(duration));  //animaatio
    }

    private IEnumerator StarpowerCoroutine(float duration)
    {
        PlayStarSound(starAudio);  //Soitetaan tähden ääni
        yield return new WaitForSeconds(duration);  //Odotetaan tähtivoiman keston ajan
        starAudioSource.Stop();  //Pysäytetään ääni, kun tähtivoima loppuu
    }

    private IEnumerator StarpowerAnimation(float duration)
    {
        starpower = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);  //Määritetään hahmo muuttamaan väriä kun tähtivoima on aktivoitu. Suluissa on määritelty värisävyt.
            }
            
            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;  //Palautetaan hahmon väri takaisin alkuperäiseksi.
        starpower = false;
    }

    public void Magicpower(float duration = 600f)  //Tähtivoima kestää 600 sekunttia.
    {
        StartCoroutine(MagicpowerCoroutine(duration));  //audio
        StartCoroutine(MagicpowerAnimation(duration));
    }

    private IEnumerator MagicpowerCoroutine(float duration)
    {
        PlayStarSound(magicPowerAudio);  //Soitetaan taikajuoman ääni
        yield return new WaitForSeconds(duration);  //Odotetaan taikajuoman keston ajan
        starAudioSource.Stop();  //Pysäytetään ääni, kun taikajuoma loppuu
    }

    private IEnumerator MagicpowerAnimation(float duration)
    {
        magicpower = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

}


