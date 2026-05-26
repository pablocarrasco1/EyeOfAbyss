using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoleccion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item =collision.GetComponent<Item>();
        if (item != null)
        {
            item.Recoger();
        }
    }
}
