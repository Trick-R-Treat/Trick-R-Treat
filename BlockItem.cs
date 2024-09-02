using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()  // Jotta MysteryBox ja sen sis‰‰n piilotettu esine ei kolise kesken‰‰n, t‰ytyy joitakin komponentteja sulkea hetkeksi.
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();  // T‰t‰ k‰ytet‰‰n vain havaitsemaan milloin pelaaja todella koskettaa kohdetta ja ker‰‰ esineen.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.isKinematic = true;  // Rigidbodya ei voi sulkea, mutta sen voi asettaa kinemaattiseksi, mik‰ tarkoittaa, ett‰ fysiikkamoottori ei t‰llˆin simuloi fysiikkaa.
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.25f);  // Pys‰ytet‰‰n komponentit nelj‰sosa sekuntiksi.

        spriteRenderer.enabled = true; // Otetaan t‰m‰ k‰yttˆˆn uudelleen.

        // Aloitetaan animointi...
        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition + Vector3.up;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = endPosition;

        // Lopuksi palautetaan kaikki takaisin toimintaan.
        rigidbody.isKinematic = false;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }

}

