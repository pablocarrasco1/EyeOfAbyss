using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaFragil : MonoBehaviour
{
    public float esperaCaida = 1f;
    public float esperaDest = 1f;

    private Vector3 posicionInicial;

    bool seCae;
    Rigidbody2D rb;

    void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        Controlador.OnReset += ResetPlataforma;
    }

    void OnDestroy()
    {
        Controlador.OnReset -= ResetPlataforma;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!seCae && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Caer());
        }
    }

    private IEnumerator Caer()
    {
        seCae = true;
        yield return new WaitForSeconds(esperaCaida);
        rb.bodyType = RigidbodyType2D.Dynamic; // Cambia su estado para que caiga
        yield return new WaitForSeconds(esperaDest);
        gameObject.SetActive(false);
    }

    // Devuelve la plataforma a su estado y posición original al reiniciar el nivel
    public void ResetPlataforma()
    {
        StopAllCoroutines(); // Detiene la caída

        seCae = false;
        transform.position = posicionInicial;
        rb.bodyType = RigidbodyType2D.Static; // Vuelve a congelarla en el aire

        gameObject.SetActive(true);
    }

}
