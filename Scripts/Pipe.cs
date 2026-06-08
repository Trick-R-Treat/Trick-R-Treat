using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;
    public bool useYButton;  //Gamepad

    public AudioClip pipe;
    private AudioSource audioSource;

    private Transform playerInPipe;
    
    private bool isEntering;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //KEYBOARD
        //if (playerInPipe != null && connection != null && !isEntering && Input.GetKeyDown(enterKeyCode))
        //{
        //    StartCoroutine(Enter(playerInPipe));
        //    audioSource.PlayOneShot(pipe);
        //}

        //KEYBOARD & GAMEPAD
        if (playerInPipe != null && connection != null && !isEntering && (Input.GetKeyDown(enterKeyCode)
        || (!useYButton && Input.GetKeyDown(KeyCode.JoystickButton1))
        || (useYButton && Input.GetKeyDown(KeyCode.JoystickButton3))))
        {
            StartCoroutine(Enter(playerInPipe));
            audioSource.PlayOneShot(pipe);
        }

        //if (Input.GetKeyDown(KeyCode.JoystickButton1))
        //    Debug.Log("B");

        //if (Input.GetKeyDown(KeyCode.JoystickButton3))
        //    Debug.Log("Y");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPipe = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPipe = null;
        }
    }

    private IEnumerator Enter(Transform player)
    {
        if (connection == null)
            yield break;

        isEntering = true;

        player.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(1f);

        bool underground = connection.position.y < 0f;
        Camera.main.GetComponent<SideScrolling>().SetUnderground(underground);

        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        player.GetComponent<PlayerMovement>().enabled = true;
        isEntering = false;
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;

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
