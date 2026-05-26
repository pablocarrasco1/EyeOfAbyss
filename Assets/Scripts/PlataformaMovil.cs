using System.Collections;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;
    public float tiempoEspera = 1f;

    private Vector3 siguientePosicion;
    private bool esperando = false;

    void Start()
    {
        siguientePosicion = puntoB.position;
    }

    void Update()
    {
        if (esperando) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            siguientePosicion,
            velocidad * Time.deltaTime
        );

        // Pausa al llegar al destino
        if (Vector3.Distance(transform.position, siguientePosicion) < 0.01f)
        {
            StartCoroutine(EsperarYCambiarDestino());
        }
    }

    // Alterna el destino entre A y B
    private IEnumerator EsperarYCambiarDestino()
    {
        esperando = true;

        yield return new WaitForSeconds(tiempoEspera);

        siguientePosicion = (siguientePosicion == puntoA.position)
            ? puntoB.position
            : puntoA.position;

        esperando = false;
    }

    // Hacemos al jugador "hijo" de la plataforma para que no se resbalarse
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform, true);
        }
    }
    
    // Libera al jugador cuando salta o se baja de la plataforma
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null, true);
        }
    }

    // Si la plataforma se desactiva, libera a cualquier objeto que tuviera encima
    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            child.SetParent(null, true);
        }
    }
}
