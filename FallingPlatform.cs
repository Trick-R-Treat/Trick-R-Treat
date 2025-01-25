using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 1f;  //Putoamisviive
    private float destroyDelay = 2f;  //Tuhoamisviive

    [SerializeField] private Rigidbody2D rb;

    //Tarkistetaan putosiko jokin alustaan.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //Jos kyseessä on pelaaja...
        {
            StartCoroutine(Fall());  //...alusta putoaa.
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);  //Odotetaan hetki.
        rb.bodyType = RigidbodyType2D.Dynamic;  //Asetetaan jäykkärunko dynaamiseksi, jotta se putoaa.
        Destroy(gameObject, destroyDelay);  //Tuhotaan objekti kun se on pudonnut.
    }
}
