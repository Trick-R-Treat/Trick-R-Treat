using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    public float height = 6.5f;  //Kameran normaalikorkeus.
    public float undergroundHeight = -9.5f;  //Kameran korkeus maanalla.

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform; //Pyydet‰‰n Unitya etsim‰‰n pelaaja, jota kamera seuraa.
    }

    private void LateUpdate()  //Seuraa hahmon p‰ivitetty‰ sijaintia.
    {
        Vector3 cameraPosition = transform.position;
        //cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);  //T‰ss‰ m‰‰ritet‰‰n, ett‰ kamera liikkuu hahmon mukana vain oikealle.
        cameraPosition.x = player.position.x;  //Jos halutaan, ett‰ kamera liikkuu hahmon mukana oikealle ja vasemmalle, niin k‰ytet‰‰n t‰t‰ vaihtoehtoa.
        transform.position = cameraPosition;
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;  //T‰m‰ on kolmeosainen lauseke. Se on v‰h‰n kuin if- ja if else-lause,
        transform.position = cameraPosition;                          //jossa on jokin ehto. T‰ss‰ "underground" on ehto.
                                                                      //Jos ehto on tosi, se lasketaan "undergroundHeight" eli maanalaisen korkeuden mukaan.
                                                                      //Jos ehto on ep‰tosi, se lasketaan "height" eli maatason korkeuden mukaan.
    }
}
