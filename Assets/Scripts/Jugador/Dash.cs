using UnityEngine;

public class DashRigidbody : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 30f;        // Velocidad del dash
    public float dashDuration = 0.2f;    // Duración en segundos
    public float dashCooldown = 1f;      // Tiempo de recarga

    [Header("Ground Check Settings")]
    public float groundCheckDistance = 1.1f; // Distancia del raycast para detectar el suelo

    private Rigidbody rb;
    private bool isDashing = false;
    private bool canDash = true;             // Controla si se puede hacer dash después de saltar o al tocar el suelo
    private bool isGrounded = false;         // Detecta si el jugador está en el suelo
    public float dashEndTime = 0f;
    public float lastDashTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {


        // Input para dash, solo si puede hacer dash y ya pasó el cooldown
        if (Input.GetKeyDown(KeyCode.E) && Time.time > lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        // Durante el dash, mueve el rigidbody hacia adelante a alta velocidad
        if (isDashing && Time.time < dashEndTime)
        {
            rb.linearVelocity = transform.forward * dashSpeed;
        }
        else if (isDashing)
        {
            EndDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        lastDashTime = Time.time;

        rb.useGravity = false;  // Opcional: desactiva la gravedad durante el dash
    }

    void EndDash()
    {
        isDashing = false;
        rb.useGravity = true;

        // Opcional: reduce la velocidad después del dash para que no quede flotando
        rb.linearVelocity *= 0.5f;
    }

    // Visualizador del raycast en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
