using UnityEngine;

public static class Extensions  //Laajennusmenetelmät vaativat, että ne määritellään staattisessa luokassa, jota se ei peri mistään.
{

    //Pelaajan Layer (oletustaso) on käyty Inspectorissa muuttamassa Defaultista Playeriksi ja tässä pyydetään sen "maski" hakemaan.
    private static LayerMask layerMask = LayerMask.GetMask("Default");

    //Tässä halutaan laajentaa Rigidbody2D ja lähettää se Vector2 suuntaan. Raycast on "kutsumanimi" jolla voidaan tätä kutsua tässä ja muissa scripteissa.
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        //Kinematic tarkoittaa, että fysiikan moottori ei ohjaa sitä.
        if (rigidbody.isKinematic)  //Jos hahmo on paikoillaan...
        {
            return false;   //...palautetaan arvo epätosi.
        }
        float radius = 0.25f;  //Hahmon koko on 1 yksikkö, joten säde on neljäsosa siitä.
        float distance = 0.375f;

        //Käytetään Unityn omaa fysiikkaa, jossa huomioidaan hahmon sijainti, säde, suunta ja etäisyys.
        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}