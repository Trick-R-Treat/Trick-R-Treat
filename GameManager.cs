using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)  //Jos instanssi ei ole nolla (sellainen on jo luotu)...
        {
            DestroyImmediate(gameObject);  //...tuhoa v‰littˆm‰sti peliobjekti.
        }
        else  //Muuten...
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //...‰l‰ tuhoa (t‰t‰ peliobjektia kun lataamme toisen kohtauksen).
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;  //M‰‰ritet‰‰n kuvan nopeudeksi 60FPS. T‰m‰n koodirivin voi poistaa jos haluaa, jolloin kuvannopeus on paljon suurempi.
        {
            NewGame();
        }
    }


    public void NewGame()
    {
        lives = 3;  //Uuden pelin k‰ynnistyess‰ hahmolla on 3 el‰m‰‰.
        coins = 0;  //Kolikoita on pelin alussa nolla.
        LoadLevel(1, 1);  //Ladataan taso 1.
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
        MusicManager.Instance.PlayBackgroundMusic(world, stage);
    }

    public void NextLevel()
    {
        //K‰ytet‰‰n t‰t‰ kun peliss‰ on useita tasoja.
        if (world == 1 && stage == 2)
        {
            LoadLevel(world + 1, 1);
        }

        //K‰ytet‰‰n t‰t‰ kun olemassa on vain yksi taso.
        //LoadLevel(world, stage + 1);

        //if (world == 1 && stage == 2)
        //{
        //    EndGame();  //Kutsu EndGame-metodia tason 2 j‰lkeen.
        //}
        //else
        //{
        //    LoadLevel(world, stage + 1);  //Muuten lataa seuraava taso.
        //}
    }

    public void LevelComplete()
    {
        MusicManager.Instance.StopBackgroundMusic();
    }

    public void ResetLevel(float delay)  //Kun pelaaja kuolee viivytet‰‰n sit‰ ennen kuin peli ladataan uudelleen.
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()  //T‰m‰ ladataan kun pelaaja kuolee.
    {
        lives--;

        if (lives > 0)  //Jos el‰mi‰ on enemm‰n kuin nolla j‰ljell‰...
        {
            LoadLevel(world, stage);  //...ladataan taso uudelleen.
        }
        else  //Muuten peli p‰‰ttyy.
        {
            GameOver();
        }
    }
    
    private void GameOver()
    {
        //NewGame();
        //Invoke(nameof(NewGame), 3f);  //Pelin p‰‰tytty‰ uusi peli k‰ynnistet‰‰n 3 sekuntin kuluttua.
        MusicManager.Instance.PlayGameOverMusic(); // Kutsutaan PlayGameOverMusic-metodia.
        SceneManager.LoadScene("GameOver");  //Ladataan GameOver -skene.
    }

    //private void EndGame()
    //{
    //    SceneManager.LoadScene("End");  //Lataa EndScene -kohtaus.
    //}

    public void AddCoin()  //Kolikko
    {
        coins++;

        if (coins == 100)  //Jos kolikoita on ker‰tty 100kpl...
        {
            AddLife();  //...lis‰t‰‰n el‰m‰...
            coins = 0;  //...ja kolikot nollataan.
        }
    }

    public void AddLife()
    {
        lives++;
    }
}


