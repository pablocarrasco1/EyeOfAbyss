using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int vidaMax = 5;
    private int vidaActual;
    public VidaUI vidaUI;

    private SpriteRenderer sr;

    public static event Action JugadorPierde;

    // Invencibilidad temporal para el dash y el cooldown de daño
    public bool EsInvencible { get; set; } = false;

    [Header("Invencibilidad tras daño")]
    public float tiempoInvencible = 0.25f; // Segundos de gracia tras recibir daño
    private bool invenciblePorDanio = false;

    void Start()
    {
        ResetVida();
        sr = GetComponent<SpriteRenderer>();
        Controlador.OnReset += ResetVida;
        ObjetoVida.OnVidaCollect += Curar;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EsInvencible || invenciblePorDanio) return;

        Trampa trampa = collision.GetComponent<Trampa>();
        if (trampa && trampa.danio > 0)
            RecibirDanio(trampa.danio);
    }

    void Curar(int cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMax) vidaActual = vidaMax;
        vidaUI.ActualizarCorazones(vidaActual);
    }

    void ResetVida()
    {
        EsInvencible = false;
        invenciblePorDanio = false;
        vidaActual = vidaMax;
        vidaUI.SetCorazonesMax(vidaMax);
    }

    private void RecibirDanio(int danio)
    {
        vidaActual -= danio;
        vidaUI.ActualizarCorazones(vidaActual);

        StartCoroutine(CooldownDanio());

        if (vidaActual <= 0)
            JugadorPierde.Invoke();
    }

    private IEnumerator CooldownDanio()
    {
        invenciblePorDanio = true;

        // Parpadeo visual durante el periodo de invencibilidad
        float tiempoRestante = tiempoInvencible;
        while (tiempoRestante > 0f)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            tiempoRestante -= 0.2f;
        }

        sr.color = Color.white;
        invenciblePorDanio = false;
    }
}