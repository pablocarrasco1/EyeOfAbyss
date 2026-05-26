using System;
using UnityEngine;

public class ObjetoLlave : MonoBehaviour, Item
{
    public static event Action OnLlaveCollect;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        Controlador.OnReset += ResetItem;
    }

    public void Recoger()
    {
        OnLlaveCollect?.Invoke();

        gameObject.SetActive(false);
    }

    public void ResetItem()
    {
        transform.position = posicionInicial;
        gameObject.SetActive(true);
    }
}
