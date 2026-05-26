using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Controlador : MonoBehaviour
{
    public GameObject pantallaGameOver;
    public GameObject pantallaFinal;
    public Transform jugador;

    private Vector3 posicionInicialJugador;

    public static event Action OnReset;

    void Start()
    {   
        VidaJugador.JugadorPierde += GameOver;
        pantallaGameOver.SetActive(false);
        pantallaFinal.SetActive(false);

        // Guardamos la posición inicial del jugador
        posicionInicialJugador = jugador.position;

        ObjetoLlave.OnLlaveCollect += NivelSuperado;
    }

    void GameOver()
    {
        pantallaGameOver.SetActive(true);
        Time.timeScale = 0f; // Todo se detiene
    }

    void NivelSuperado()
    {
        pantallaFinal.SetActive(true);
        Time.timeScale = 0f; // Todo se detiene
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MenuInicio");
    }

    public void ResetJuego()
    {
        pantallaGameOver.SetActive(false);
        Time.timeScale = 1f;

        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Devolver al jugador a la posición inicial
        jugador.position = posicionInicialJugador;

        // Resetear objetos y vida
        OnReset?.Invoke();
    }
}
