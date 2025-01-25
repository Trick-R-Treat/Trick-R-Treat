using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;  //Kolikko
    public Sprite emptyBlock;
    public int maxHits = -1;  //Negatiivinen luku antaa mahdollisuuden iskeä kohdetta loputtomasti.

    private bool animating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))  //Jos kohde ei ole animoitu ja maksimi osuma ei ole sama kuin nolla ja kyseessä on pelaaja...
        {
            if (collision.transform.DotTest(transform, Vector2.up))  //Jos törmäys kohdistuu ylöspäin...
            {
                Hit();  //...kohdetta voi lyödä.
            }
        }
    }

    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;  //MysteryBlock >> Inspector > ota täppä pois Sprite Renderer nimen vierestä, niin MysteryBoxista tulee näkymätön. 
        
        maxHits--;

        if (maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }
        
        if (item != null)  //Kolikko
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()  //Tämä vaatii koodin alkuun using System.Collections;
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }
}

