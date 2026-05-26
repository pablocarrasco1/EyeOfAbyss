using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    public Rigidbody2D rb;
    private VidaJugador vidaJugador;
    bool miraDerecha = true;

    [Header("Movimiento")]
    public float velMovimiento = 5f;
    [SerializeField] private float movimientoHorizontal;

    [Header("Salto")]
    public float potenciaSalto = 10f;
    public int maxSaltos = 2;
    [SerializeField] private int saltosRestantes;

    [Header("Detección de Suelo")]
    public Transform posCheckTierra;
    public Vector2 tamCheckTierra = new Vector2(0.5f, 0.05f);
    public LayerMask capaTierra;
    bool tocaTierra;

    [Header("Gravedad")]
    public float gravedadBase = 2f;
    public float velCaidaMax = 18f;
    public float velCaidaMultiplicador = 2f;

    [Header("Detección de Paredes")]
    public Transform posCheckPared;
    public Vector2 tamCheckPared = new Vector2(0.05f, 0.7f);
    public LayerMask capaPared;

    [Header("Deslizamiento en Pared")]
    public float velDeslizarPared = 2f;
    bool seDesliza;

    [Header("Salto en Pared")]
    bool saltaEnPared;
    float direccionSaltoPared;
    public float tiempoSaltoPared = 0.2f;
    float timerSaltoPared;
    public Vector2 potenciaSaltoPared = new Vector2(5f, 10f);

    [Header("Dash")]
    public float velocidadDash = 18f;
    public float duracionDash = 0.15f;
    public float cooldownDash = 0.8f;
    [SerializeField] private bool dashDisponible = true;
    [SerializeField] private bool estaDasheando = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        vidaJugador = GetComponent<VidaJugador>();
    }

    void FixedUpdate()
    {
        CheckTierra();

        if (estaDasheando) return;

        Gravedad();
        GravedadPared();
        GravedadSaltoPared();

        if (!saltaEnPared)
        {
            rb.linearVelocity = new Vector2(movimientoHorizontal * velMovimiento, rb.linearVelocity.y);
            Voltear();
        }
    }

    private void Gravedad()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravedadBase * velCaidaMultiplicador;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -velCaidaMax));
        }
        else
        {
            rb.gravityScale = gravedadBase;
        }
    }

    private void GravedadPared()
    {
        if (!tocaTierra && CheckPared() && movimientoHorizontal != 0)
        {
            seDesliza = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -velDeslizarPared));
        }
        else
        {
            seDesliza = false;
        }
    }

    private void GravedadSaltoPared()
    {
        if (seDesliza)
        {
            saltaEnPared = false;
            direccionSaltoPared = -transform.localScale.x; // El salto te empuja en dirección opuesta a la pared
            timerSaltoPared = tiempoSaltoPared;

            CancelInvoke(nameof(CancelarSaltoPared));
        }
        else if (timerSaltoPared > 0f)
        {
            timerSaltoPared -= Time.deltaTime;
        }
    }

    private void CancelarSaltoPared()
    {
        saltaEnPared = false;
    }

    public void Mover(InputAction.CallbackContext context)
    {
        movimientoHorizontal = context.ReadValue<Vector2>().x;
    }

    public void Saltar(InputAction.CallbackContext context)
    {
        if (estaDasheando) return;

        if (saltosRestantes > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, potenciaSalto);
                saltosRestantes--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                saltosRestantes--;
            }
        }

        // Salto Pared
        if (context.performed && timerSaltoPared > 0f)
        {
            saltaEnPared = true;
            rb.linearVelocity = new Vector2(direccionSaltoPared * potenciaSaltoPared.x, potenciaSaltoPared.y);
            timerSaltoPared = 0;

            if (transform.localScale.x != direccionSaltoPared)
            {
                miraDerecha = !miraDerecha;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelarSaltoPared), tiempoSaltoPared + 0.1f);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!dashDisponible || estaDasheando) return;
        if (movimientoHorizontal == 0f) return;

        float direccion = movimientoHorizontal > 0f ? 1f : -1f;
        StartCoroutine(EjecutarDash(direccion));
    }

    private IEnumerator EjecutarDash(float direccion)
    {
        dashDisponible = false;
        estaDasheando = true;

        if (vidaJugador != null) vidaJugador.EsInvencible = true;

        // Anulamos gravedad para que la trayectoria del Dash sea completamente horizontal
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(direccion * velocidadDash, 0f);

        yield return new WaitForSeconds(duracionDash);

        estaDasheando = false;
        rb.gravityScale = gravedadBase;

        if (vidaJugador != null) vidaJugador.EsInvencible = false;

        yield return new WaitForSeconds(cooldownDash);
        dashDisponible = true;
    }

    private void CheckTierra()
    {
        if (Physics2D.OverlapBox(posCheckTierra.position, tamCheckTierra, 0, capaTierra))
        {
            saltosRestantes = maxSaltos;
            tocaTierra = true;
            dashDisponible = true;
        }
        else
        {
            tocaTierra = false;
        }
    }

    private bool CheckPared()
    {
        return Physics2D.OverlapBox(posCheckPared.position, tamCheckPared, 0, capaPared);
    }

    private void Voltear()
    {
        if (miraDerecha && movimientoHorizontal < 0 || !miraDerecha && movimientoHorizontal > 0)
        {
            miraDerecha = !miraDerecha;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (posCheckTierra != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(posCheckTierra.position, tamCheckTierra);
        }

        if (posCheckPared != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(posCheckPared.position, tamCheckPared);
        }
    }
}