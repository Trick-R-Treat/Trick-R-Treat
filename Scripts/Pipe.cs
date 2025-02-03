using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;  //M‰‰ritet‰‰n minne putki johtaa.
    public KeyCode enterKeyCode = KeyCode.S;  //M‰‰ritet‰‰n S-n‰pp‰in, jolla Mario menee putkeen.
    public Vector3 enterDirection = Vector3.down;  //M‰‰ritet‰‰n suunta "alas".
    public Vector3 exitDirection = Vector3.zero;  //M‰‰ritet‰‰n poistumissuunta, joka on nolla.

    public AudioClip pipe;
    private AudioSource audioSource;

    void Start()  //AUDIO
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip clip)  //AUDIO
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.CompareTag("Player"))  //Jos yhteys ei ole nolla ja kyseess‰ on pelaaja...
        {
            if (Input.GetKeyDown(enterKeyCode))  //Jos alas-n‰pp‰int‰ on painettu...
            {
                StartCoroutine(Enter(other.transform));
                PlaySound(pipe);
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;  //Poistetaan pelaajan liikeanimointi k‰ytˆst‰.

        Vector3 enteredPosition = transform.position + enterDirection;  //T‰m‰ animoi hahmon siihen suuntaan johon se on siirtym‰ss‰.
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(1f);  //Viivytet‰‰n siirtymist‰ 1 sekuntin verran.

        bool underground = connection.position.y < 0f;  //Jos sijainti johon pelaaja on yhteydess‰ on pienempi kuin nolla, se on maanalainen ja tuloksena on tosi.
        Camera.main.GetComponent<SideScrolling>().SetUnderground(underground);  //Kamera seuraa pelaajaa maanalle.

        if (exitDirection != Vector3.zero)  //Jos poistumissuunta ei ole nolla...
        {
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        player.GetComponent<PlayerMovement>().enabled = true;  //Otetaan pelaajan liikeanimointi k‰yttˆˆn.
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;  //Animoinnin kesto on 1 sekuntti.

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }
}
