using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer {  get; private set; }
    private PlayerMovement movement;

    // N‰m‰ ilmestyy hahmon (Marion) Inspectoriin, jolloin hahmon animaatiot voi raahata Sprites -kansiosta oikeisiin kohtiin.
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprite run;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();  // Haetaan Parent -komponentti.
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

    private void LateUpdate()  // P‰ivitet‰‰n hahmon ulkon‰kˆ vasta sen j‰lkeen kun kaikki erilaiset fysiikka- ja liiketilat on p‰ivitetty.
    {
        run.enabled = movement.running;

        if (movement.jumping)  // Jos hahmo hypp‰‰...
        {
            spriteRenderer.sprite = jump;  //...k‰ytet‰‰n jump-animaatiota.
        }
        else if (movement.sliding)  // Jos hahmo liukuu...
        {
            spriteRenderer.sprite = slide;  //...k‰ytet‰‰n liukumis-animaatiota.
        }
        else if (!movement.running)  // Jos hahmo ei juokse, k‰ytet‰‰n toimetonta animaatiota.
        {
            spriteRenderer.sprite = idle;
        }

    }
}
