using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaUI : MonoBehaviour
{
    public Image corazonPrefab;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    private List<Image> corazones = new List<Image>();
    
    public void SetCorazonesMax(int maxCorazones)
    {
        // Limpieza de los corazones previos en la UI
        foreach(Image corazon in corazones)
        {
            Destroy(corazon.gameObject);
        }

        corazones.Clear();

        for (int i = 0; i < maxCorazones; i++)
        {
            Image nuevoCorazon = Instantiate(corazonPrefab, transform);
            nuevoCorazon.sprite = corazonLleno;
            nuevoCorazon.color = Color.red;
            corazones.Add(nuevoCorazon);
        }
    }

    // Recorre la lista de corazones para llenar o vaciar los iconos según el daño recibido
    public void ActualizarCorazones(int vidaActual)
    {
        for (int i = 0; i < corazones.Count; i++)
        {
            if (i < vidaActual)
            {
                corazones[i].sprite = corazonLleno;
                corazones[i].color = Color.red;

            }
            else
            {
                corazones[i].sprite = corazonVacio;
                corazones[i].color = Color.white;
            }
        }
        
    }
}
