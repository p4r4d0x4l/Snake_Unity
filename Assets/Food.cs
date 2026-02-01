using UnityEngine;

/* 
 * Dette skriptet sørger for at matobjektets (food) posisjon tilfeldiggjøres ved spillets oppstart,
 * samt hver gang snake spiser det.
 */

// ----------------------------------------------------------------------------------------------------------------------
//                                       --- TILFELDIGJØR POSISJON FRA START ---
// ----------------------------------------------------------------------------------------------------------------------

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;      // Referanse til BoxCollider2D-glideren i Unity-editoren

    // Funksjon som Unity automatisk påkaller (Den første rammen dette skriptet er aktivert på et spillobjekt):
    private void Start()    // Start() kalles én gang før første Update() - etter at GameObject er aktivert
    {
        RandomizePosition();    // Tilfeldiggjør posisjon ved spilloppstart
    }

    private void RandomizePosition()
    {
        // BoxCollider2D inneholder en egenskap; bounds - som brukes til å generere tilfeldige verdier:
        Bounds bounds = this.gridArea.bounds;

        // Genererer et tilfedig tall innenfor perimeteren til bounds i både x- og y-aksen definert i editoren:
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        
        // Tilordner food's posisjon til respektive koordinater og avrundes for å sikre korrekt justering til rutenettet:
        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);    // z-akse satt til 0
    }


    // ===========================================================================================================================


    // ---------------------------------------------------------------------------------------------------------------------------
    //                            --- TILFELDIGJØR POSISJON PÅ NYTT HVER GANG SNAKE SPISER DEN --- 
    // ---------------------------------------------------------------------------------------------------------------------------

    // OnTriggerEnter2D påkalles automatisk hver gang spillobjektene kolliderer med hverandre:
    private void OnTriggerEnter2D(Collider2D other)    // Collider2D gir oss en referane til det andre objektet i kollisjonen
    {
        if (other.tag == "Player")  // Sjekker hvilket annet objekt som kolliderte med food (her: kun snake) med Unitys tag-system
        {
            RandomizePosition();    // Påkaller så RandomizePosition-funksjonen
        }        
    }
}
