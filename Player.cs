using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer; //T�t� tarvitaan seuraamaan kumpaa yll�olevista rendereist� milloinkin k�ytet��n.

    private DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;  //Pienen hahmon collider on liian pieni isolle hahmolle, joten se pit�� m��ritt�� erikseen koodissa.

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

        starAudioSource = gameObject.AddComponent<AudioSource>();  //Erillinen audio komponentti t�htivoiman ��nelle.
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
        if (!dead && !starpower && !magicpower)  //Jos hahmo ei ole kuollut ja sill� ei ole t�htivoimaa...
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
        smallRenderer.enabled = false;  //Poistetaan pienen hahmon render�inti k�yt�st�.
        bigRenderer.enabled = false;  //Poistetaan suuren hahmon render�inti k�yt�st�.
        deathAnimation.enabled = true;  //Otetaan hahmon kuolema-animaatio k�ytt��n.
        PlaySound(death);

        GameManager.Instance.ResetLevel(3f);  //Nollataan taso 3 sekuntin kuluttua. T�h�n on t�rke� m��ritt�� aika, jotta kuolema-animaatiolla on aikaa rullata l�pi.
    }

    public void Grow()
    {
        smallRenderer.enabled = false;  //Otetaan pienen hahmon renderer pois k�yt�st�.
        bigRenderer.enabled = true;  //Otetaan suuren hahmon renderer k�ytt��n.
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);  //T�ss� kasvatetaan collider yhdest� yksik�st� kahteen.
        capsuleCollider.offset = new Vector2(0f, 0.5f);  //T�ss� siirret��n collideria puoli yksikk�� yl�sp�in, jotta se kohdistuu oikein.

        StartCoroutine(ScaleAnimation());
        PlaySound(powerUp);  //Soitetaan ��ni kun hahmo kasvaa.
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

        while (elapsed < duration)  //Kun on kulunut v�hemm�n kuin kesto jatkamme animointia...
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

    public void Starpower(float duration = 10f)  //T�htivoima kest�� 10 sekunttia.
    {
        StartCoroutine(StarpowerCoroutine(duration));  //audio
        StartCoroutine(StarpowerAnimation(duration));  //animaatio
    }

    private IEnumerator StarpowerCoroutine(float duration)
    {
        PlayStarSound(starAudio);  //Soitetaan t�hden ��ni
        yield return new WaitForSeconds(duration);  //Odotetaan t�htivoiman keston ajan
        starAudioSource.Stop();  //Pys�ytet��n ��ni, kun t�htivoima loppuu
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
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);  //M��ritet��n hahmo muuttamaan v�ri� kun t�htivoima on aktivoitu. Suluissa on m��ritelty v�ris�vyt.
            }
            
            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;  //Palautetaan hahmon v�ri takaisin alkuper�iseksi.
        starpower = false;
    }

    public void Magicpower(float duration = 600f)  //T�htivoima kest�� 600 sekunttia.
    {
        StartCoroutine(MagicpowerCoroutine(duration));  //audio
        StartCoroutine(MagicpowerAnimation(duration));
    }

    private IEnumerator MagicpowerCoroutine(float duration)
    {
        PlayStarSound(magicPowerAudio);  //Soitetaan taikajuoman ��ni
        yield return new WaitForSeconds(duration);  //Odotetaan taikajuoman keston ajan
        starAudioSource.Stop();  //Pys�ytet��n ��ni, kun taikajuoma loppuu
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

