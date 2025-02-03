using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public ParticleSystem fireworks;  //Viittaus Particle System -objektiin.
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;

    public AudioClip flagPole;  //AUDIO
    public AudioClip fireWorks;  //AUDIO
    private AudioSource audioSource;  //AUDIO

    void Start()  //AUDIO
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip clip)  //AUDIO
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  //Jos kyseess‰ on pelaaja...
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));  //Siirret‰‰n lippua kohti lipputangon alaosaa.
            StartCoroutine(LevelCompleteSequence(other.transform));  //V‰litet‰‰n pelaajan muunnos...
            PlaySound(flagPole);  //AUDIO
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;  //Poistetaan pelaajan liikeanimointi k‰ytˆst‰.

        //Laitetaan pelaaja liukumaan lipputankoa pitkin alas ja siirtym‰‰n linnan oviaukolle.
        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position + Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        fireworks.Play();  //K‰ynnistet‰‰n ilotulitus
        PlaySound(fireWorks);  //AUDIO

        GameManager.Instance.AddScore(1000);

        player.gameObject.SetActive(false);  //Kun pelaaja on saavuttanut linnan oviaukon, poistetaan pelaajan peliobjekti k‰ytˆst‰.

        yield return new WaitForSeconds(10f);  //Odotetaan 10 sekunttia ennen kuin uusi taso k‰ynnistet‰‰n.

        GameManager.Instance.LoadLevel(nextWorld, nextStage);

        fireworks.Stop();
    }

    private IEnumerator MoveTo(Transform subject, Vector3 destination)  //Siirret‰‰n lippu ja pelaaja haluttuun paikkaan.
    {
        //Kun vectorin et‰isyys objektin sijainnin ja m‰‰r‰np‰‰n v‰lill‰ on suurempi kuin kahdeksasosa yksikˆst‰...
        //objekti on liian kaukana, eik‰ se ole tavoittanut m‰‰r‰np‰‰t‰. T‰ss‰ tapauksessa jatketaan animointia.
        while (Vector3.Distance(subject.position, destination) > 0.128f)                                                                 
        {
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);  //Kun objekti on kaukana m‰‰r‰np‰‰st‰, p‰ivitet‰‰n objektin
            yield return null;  //Suljetaan kehys.                                                          //sijainti. Siirret‰‰n objektia kohti m‰‰r‰np‰‰t‰.
        }

        subject.position = destination;  //Kun objekti on melko l‰hell‰, voidaan asettaa sen sijainti m‰‰r‰np‰‰ksi.
    }
}
