using System;
using UnityEngine;
using TMPro;

public class CronometroNivel : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoTiempo;

    private float tiempoTranscurrido = 0f;
    private bool corriendo = false;

    // Almacena el tiempo final para que PantallaFinalControlador lo lea
    public static float TiempoFinal { get; private set; }

    public static event Action<float> OnNivelCompletado;

    void Start()
    {
        TiempoFinal = 0f;
        VidaJugador.JugadorPierde += DetenerPorDerrota;
        ObjetoLlave.OnLlaveCollect += DetenerPorVictoria;
        Controlador.OnReset += ReiniciarCronometro;
        Iniciar();
    }

    void OnDestroy()
    {
        VidaJugador.JugadorPierde -= DetenerPorDerrota;
        ObjetoLlave.OnLlaveCollect -= DetenerPorVictoria;
        Controlador.OnReset -= ReiniciarCronometro;
    }

    void Update()
    {
        if (!corriendo) return;

        // Suma el tiempo transcurrido frame a frame y actualiza el texto
        tiempoTranscurrido += Time.deltaTime;
        ActualizarTexto();
    }

    private void Iniciar()
    {
        tiempoTranscurrido = 0f;
        corriendo = true;
    }

    private void DetenerPorVictoria()
    {
        corriendo = false;
        TiempoFinal = tiempoTranscurrido;
        OnNivelCompletado?.Invoke(TiempoFinal);
    }

    private void DetenerPorDerrota()
    {
        corriendo = false;
    }

    private void ReiniciarCronometro()
    {
        Iniciar();
    }

    private void ActualizarTexto()
    {
        if (textoTiempo == null) return;
        textoTiempo.text = FormatearTiempo(tiempoTranscurrido);
    }

    public static string FormatearTiempo(float segundos)
    {
        // Convierte segundos a formato legible
        int min = (int)(segundos / 60f);
        int seg = (int)(segundos % 60f);
        int ms  = (int)((segundos - Mathf.Floor(segundos)) * 100f);
        return $"{min:00}:{seg:00}.{ms:00}";
    }
}
