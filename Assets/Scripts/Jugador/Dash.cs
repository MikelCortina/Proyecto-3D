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
    private float dashEndTime = 0f;
    private float lastDashTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Actualizamos el estado de grounded usando el mismo método que en PlayerMovement
        CheckGrounded();

        // Si está en el suelo, habilita el dash otra vez
        if (isGrounded)
        {
            canDash = true;
        }

        // Input para dash, solo si puede hacer dash y ya pasó el cooldown
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Time.time > lastDashTime + dashCooldown)
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
        canDash = false;  // Solo se puede hacer dash una vez antes de tocar el suelo otra vez
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

    void CheckGrounded()
    {
        RaycastHit hit;
        // El raycast va hacia abajo desde el transform del objeto
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance);
    }

    // Visualizador del raycast en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
