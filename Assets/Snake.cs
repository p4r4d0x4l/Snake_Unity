using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // Deklarerer variabel som holder oversikt over retning (_direction):
    private Vector2 _direction = Vector2.right;     // Vector2 representerer ønsket bevegelsesretning i planet (x, y), brukt senere til å oppdatere posisjon.


    // Oppretter og initialiserer en liste som holder oversikt over alle segmentene av snake:
    private List<Transform> _segments = new List<Transform>();

    // Oppretter en referanse til prefab-en i Unity-editoren:
    public Transform segmentPrefab;

    // Setter startstørrelse på snake ved oppstart av spill eller etter resett:
    public int initialSize = 4;

    // Oppretter en Start()-funksjon for å initialisere listen:
    private void Start()
    {
        ResetState();   // Resetter spillet hver gang et nytt spill startes
    }


    // Variabel funksjon som Unity påkaller for hver eneste ramme som dette skriptet er aktivt på spillobjektet:
    private void Update()       // Avhenger av hvor bra spillet kjører på PC (bildefrekvens)
    {
        // Logikk som tilordner retning basert på bruker-input:
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;
        } 
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }
    }


    // Fast funksjon som endrer posisjon til snake-objektet basert på retnings-input fra bruker:
    private void FixedUpdate()      // Kjøres alltid med et fast tidsintervall
    {
        // For at alle segmentene skal kunne følge med resten av snake må vi endre deres posisjon i omvendt rekkefølge (fra halen til hodet)
        for (int i = _segments.Count - 1; i > 0; i--)       // ... Dette gjøres med omvendt iterering (bakvendt looping)
        {
            _segments[i].position = _segments[i - 1].position;     // Hvert segment følger nå det segmentet foran seg
        }

        // Denne punkttransformasjonen åpner transformasjonsobjektet snake (i Unity):
        this.transform.position = new Vector3(
            
            // Anrundes for å sikre at snake justeres til et rutenett (grid):
            Mathf.Round(this.transform.position.x) + _direction.x,  // Nåværende posisjon + retning i x-akse
            Mathf.Round(this.transform.position.y) + _direction.y,  // Nåværende posisjon + retning i y-akse
            0.0f    // z-aksebevegelse brukes ikke i dette spillet så settes til verdi 0
        );
    }


    // Oppretter en funksjon som initierer en kopi av prefaben (opprettet i Unity-editoren):
    private void Grow()
    {
        // Oppretter en referanse til det nye segmentet:
        Transform segment = Instantiate(this.segmentPrefab);    // ... og sender inn objektet som ønskes klonet

        // Setter posisjonen til segmentet til å være på slutten av snake-objektet (halen):
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);     // Legger segmentet til i listen
    }


    // KOLLISJONSHÅNDTERING

    private void ResetState()   // Kolliderer snake med veggene eller seg selv resettes spill-statusen
    {
        // Tømmer segmentlisten for å kunne starte på nytt:
        for (int i = 1; i < _segments.Count; i++)   // Starter på indeks 1 fordi hovedobjektet er hodet til snake (som ikke skal slettes)
        {
            Destroy(_segments[i].gameObject);   // Påkaller funksjonen Destroy og sender inn referansen til objektet som skal slettes
        }

        _segments.Clear();  // Sletter segmentlisten
        _segments.Add(this.transform);  // Legger tilbake hovedobjektet (snake-hodet)

        // Resetter snake til startstørrelsen:
        for (int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        // Tilbakesetter snake sin posisjon til start:
        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)    // Collider2D gir oss en referane til det andre objektet i kollisjonen
    {
        if (other.tag == "Food")  // Sjekker hvilket annet objekt som kolliderte med snake (her: kun food) med Unitys tag-system
        {
            Grow();    // Påkaller så Grow()-funksjonen

            // Spiller tildeles 10 poeng per matbit:
            FindAnyObjectByType<GameManager>().AddScore(10);
        }
        else if (other.tag == "Obstacle")
        {
            //ResetState();
            FindAnyObjectByType<GameManager>().GameOver();
        }
    }
}
