using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PantallaFinalControlador : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelNuevoRecord;
    public GameObject panelTabla;

    [Header("Textos de estado")]
    public TextMeshProUGUI textoTiempoFinal;
    public TextMeshProUGUI textoMensaje; // Solo visible si es record
    public float velocidadParpadeo = 0.5f;

    [Header("Selector de nombre arcade")]
    public TextMeshProUGUI[] caracterTextos;
    public Image indicadorActivo;

    [Header("Tabla de puntuaciones")]
    public Transform contenedorFilas;
    public GameObject prefabFila;

    [Header("Botones")]
    public Button botonConfirmar;
    public Button botonMenuPrincipal;

    private static readonly char[] CARACTERES = BuildAlfabeto();
    private int[] indicesChar = { 0, 0, 0 };
    private int   slotActivo  = 0;
    private bool  modoEntrada = false;
    private float tiempoJugador;
    private Coroutine coroutineParpadeo;

    void OnEnable()
    {
        tiempoJugador = CronometroNivel.TiempoFinal;

        // Solo visible si es record
        if (textoMensaje != null)
            textoMensaje.gameObject.SetActive(false);

        if (textoTiempoFinal != null)
            textoTiempoFinal.text = CronometroNivel.FormatearTiempo(tiempoJugador);

        bool esRecord = SistemaPuntuaciones.EsNuevoRecord(tiempoJugador);

        IniciarModoEntrada(esRecord);

        botonConfirmar.onClick.AddListener(ConfirmarNombre);
        botonMenuPrincipal.onClick.AddListener(IrAlMenu);
    }

    void OnDisable()
    {
        PararParpadeo();
        botonConfirmar.onClick.RemoveAllListeners();
        botonMenuPrincipal.onClick.RemoveAllListeners();
    }

    void Update()
    {
        if (!modoEntrada) return;
        ManejarInputArcade();
    }

    private void IniciarParpadeo(TextMeshProUGUI texto)
    {
        PararParpadeo();
        if (texto != null)
            coroutineParpadeo = StartCoroutine(Parpadear(texto));
    }

    private void PararParpadeo()
    {
        if (coroutineParpadeo != null)
        {
            StopCoroutine(coroutineParpadeo);
            coroutineParpadeo = null;
        }
    }

    // Corutina genérica para hacer que un texto aparezca y desaparezca
    private IEnumerator Parpadear(TextMeshProUGUI texto)
    {
        while (true)
        {
            texto.enabled = !texto.enabled;
            yield return new WaitForSecondsRealtime(velocidadParpadeo);
        }
    }

    private void IniciarModoEntrada(bool esRecord)
    {
        modoEntrada = true;
        panelNuevoRecord.SetActive(true);
        panelTabla.SetActive(false);

        if (textoMensaje != null)
        {
            if (esRecord)
            {
                textoMensaje.gameObject.SetActive(true);
                textoMensaje.text = "NuevoRecord!";
                textoMensaje.enabled = true;
                IniciarParpadeo(textoMensaje);
            }
            else
            {
                textoMensaje.gameObject.SetActive(false);
            }
        }

        // Resetea los índices de las 3 letras a la 'A'
        slotActivo = 0;
        for (int i = 0; i < 3; i++) indicesChar[i] = 0;

        RefrescarCaracteres();
        ActualizarIndicador();
    }

    private void ManejarInputArcade()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        bool subir     = kb.wKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame;
        bool bajar     = kb.sKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame;
        bool derecha   = kb.dKey.wasPressedThisFrame || kb.rightArrowKey.wasPressedThisFrame || kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame;
        bool izquierda = kb.aKey.wasPressedThisFrame || kb.leftArrowKey.wasPressedThisFrame;

        if (subir)
        {
            // Cambia la letra actual hacia atrás (bucle Z - A)
            indicesChar[slotActivo] = (indicesChar[slotActivo] - 1 + CARACTERES.Length) % CARACTERES.Length;
            RefrescarCaracteres();
        }
        else if (bajar)
        {
            // Cambia la letra actual hacia adelante (bucle A - Z)
            indicesChar[slotActivo] = (indicesChar[slotActivo] + 1) % CARACTERES.Length;
            RefrescarCaracteres();
        }
        else if (derecha)
        {
            // Pasa a la siguiente letra. Si es la última, confirma el nombre
            if (slotActivo < 2)
            {
                slotActivo++;
                ActualizarIndicador();
            }
            else
            {
                ConfirmarNombre();
            }
        }
        else if (izquierda && slotActivo > 0)
        {
            // Vuelve a la letra anterior
            slotActivo--;
            ActualizarIndicador();
        }
    }

    private void RefrescarCaracteres()
    {
        for (int i = 0; i < 3; i++)
        {
            if (caracterTextos != null && i < caracterTextos.Length && caracterTextos[i] != null)
                caracterTextos[i].text = CARACTERES[indicesChar[i]].ToString();
        }
    }

    private void ActualizarIndicador() // Mueve el puntero debajo de la letra que se está modificando
    {
        if (indicadorActivo == null || caracterTextos == null) return;

        RectTransform rt = indicadorActivo.rectTransform;
        RectTransform ct = caracterTextos[slotActivo].rectTransform;
        rt.anchoredPosition = new Vector2(ct.anchoredPosition.x, rt.anchoredPosition.y);
    }

    private void ConfirmarNombre()
    {
        modoEntrada = false;

        PararParpadeo();
        if (textoMensaje != null)
        {
            textoMensaje.enabled = true;
            textoMensaje.gameObject.SetActive(false);
        }

        string nombre = "" + CARACTERES[indicesChar[0]]
                           + CARACTERES[indicesChar[1]]
                           + CARACTERES[indicesChar[2]];

        SistemaPuntuaciones.GuardarEntrada(nombre, tiempoJugador);

        panelNuevoRecord.SetActive(false);

        MostrarTabla();
    }

    private void MostrarTabla()
    {
        panelTabla.SetActive(true);

        // Borra las filas viejas del contenedor para no duplicarlas al recargar
        foreach (Transform hijo in contenedorFilas)
            Destroy(hijo.gameObject);

        var entradas = SistemaPuntuaciones.ObtenerTabla();
        string[] sufijos = { "1ST", "2ND", "3RD", "4TH", "5TH",
                             "6TH", "7TH", "8TH", "9TH", "10TH" };

        for (int i = 0; i < entradas.Count; i++)
        {
            GameObject fila = Instantiate(prefabFila, contenedorFilas);
            TextMeshProUGUI[] textos = fila.GetComponentsInChildren<TextMeshProUGUI>();

            // Posición fija por índice — sin depender del Layout Group
            RectTransform rt = fila.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -i * 40f);

            if (textos.Length >= 3)
            {
                textos[0].text = sufijos[i];
                textos[1].text = entradas[i].nombre;
                textos[2].text = CronometroNivel.FormatearTiempo(entradas[i].tiempo);

                // Código de color según el podio (Oro, Plata, Bronce o Blanco)
                Color color = i == 0 ? new Color(1f, 0.84f, 0f)
                            : i == 1 ? new Color(0.75f, 0.75f, 0.75f)
                            : i == 2 ? new Color(0.8f, 0.5f, 0.2f)
                            : Color.white;

                // Destaca la fila del jugador mostrandola en amarillo y haciéndola parpadear
                bool esMiTiempo = Mathf.Approximately(entradas[i].tiempo, tiempoJugador);
                if (esMiTiempo) color = new Color(1f, 1f, 0f);

                foreach (var t in textos) t.color = color;

                if (esMiTiempo)
                    StartCoroutine(ParpadearFila(textos, color));
            }
        }
    }

    private IEnumerator ParpadearFila(TextMeshProUGUI[] textos, Color color)
    {
        while (true)
        {
            foreach (var t in textos) t.color = Color.clear;
            yield return new WaitForSecondsRealtime(0.4f);
            foreach (var t in textos) t.color = color;
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    private void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }

    // Genera dinámicamente un array con caracteres de la A a la Z y del 0 al 9
    private static char[] BuildAlfabeto()
    {
        var lista = new List<char>();
        for (char c = 'A'; c <= 'Z'; c++) lista.Add(c);
        for (char c = '0'; c <= '9'; c++) lista.Add(c);
        return lista.ToArray();
    }
}