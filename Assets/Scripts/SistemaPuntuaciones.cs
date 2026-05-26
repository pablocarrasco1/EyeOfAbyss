using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SistemaPuntuaciones
{
    private const int TOP_MAX = 10;
    private const string NOMBRE_ARCHIVO = "scores.json";

    [Serializable]
    public class EntradaPuntuacion
    {
        public string nombre;
        public float  tiempo;
        public string fecha;

        public EntradaPuntuacion(string nombre, float tiempo)
        {
            this.nombre = nombre.ToUpper();
            this.tiempo = tiempo;
            this.fecha  = DateTime.Now.ToString("o");
        }
    }

    // Estructura necesaria porque JsonUtility no puede serializar List<> directamente de forma nativa
    [Serializable]
    private class TablaWrapper
    {
        public List<EntradaPuntuacion> entradas = new List<EntradaPuntuacion>();
    }

    // Devuelve todas las entradas ordenadas
    public static List<EntradaPuntuacion> ObtenerTabla()
    {
        return CargarDesdeArchivo().entradas;
    }

    // Comprueba si el tiempo dado entra en el top
    public static bool EsNuevoRecord(float tiempo)
    {
        var tabla = CargarDesdeArchivo();
 
        // Si no hay tiempos registrados, es un récord
        if (tabla.entradas.Count == 0) return true;
 
        // Récord solo si supera al PRIMER puesto
        return tiempo < tabla.entradas[0].tiempo;
    }

    public static int GuardarEntrada(string nombre, float tiempo)
    {
        var tabla = CargarDesdeArchivo();
        var nueva = new EntradaPuntuacion(nombre, tiempo);

        tabla.entradas.Add(nueva);

        // Ordenar la lista por tiempo
        tabla.entradas.Sort((a, b) => a.tiempo.CompareTo(b.tiempo));

        // Recortar al máximo permitido
        if (tabla.entradas.Count > TOP_MAX)
            tabla.entradas.RemoveRange(TOP_MAX, tabla.entradas.Count - TOP_MAX);

        GuardarEnArchivo(tabla);

        return tabla.entradas.IndexOf(nueva) + 1; // 1-based
    }

    // Ruta de guardado
    private static string RutaArchivo => Path.Combine(Application.persistentDataPath, NOMBRE_ARCHIVO);

    // Lee el archivo y lo transforma de texto JSON a objetos C#
    private static TablaWrapper CargarDesdeArchivo()
    {
        if (!File.Exists(RutaArchivo)) return new TablaWrapper();

        try
        {
            string json = File.ReadAllText(RutaArchivo);
            return JsonUtility.FromJson<TablaWrapper>(json) ?? new TablaWrapper();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[SistemaPuntuaciones] Error al leer scores.json: {e.Message}");
            return new TablaWrapper();
        }
    }

    // Convierte los objetos de C# a texto estructurado JSON y lo escribe
    private static void GuardarEnArchivo(TablaWrapper tabla)
    {
        try
        {
            string json = JsonUtility.ToJson(tabla, prettyPrint: true);
            File.WriteAllText(RutaArchivo, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SistemaPuntuaciones] Error al guardar scores.json: {e.Message}");
        }
    }
}
