using System;
using UnityEngine;

public class ObjetoVida : MonoBehaviour, Item
{
    public int cantidadRecuperacion = 1;

    public static event Action<int> OnVidaCollect;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        Controlador.OnReset += ResetItem;
    }

    public void Recoger()
    {
        OnVidaCollect?.Invoke(cantidadRecuperacion);

        gameObject.SetActive(false);
    }

    public void ResetItem()
    {
        transform.position = posicionInicial;
        gameObject.SetActive(true);
    }
}
