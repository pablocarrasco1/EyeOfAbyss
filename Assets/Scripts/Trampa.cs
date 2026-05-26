using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampa : MonoBehaviour
{
    public float fuerzaRebote = 10f;
    public int danio = 1;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ManejadorBote(collision.gameObject);
        }
    }

    private void ManejadorBote(GameObject player) {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb) {
            // Reinicia la aceleracion del jugador
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            // Fuerza del rebote
            rb.AddForce(Vector2.up * fuerzaRebote, ForceMode2D.Impulse);
        }
    }
}
