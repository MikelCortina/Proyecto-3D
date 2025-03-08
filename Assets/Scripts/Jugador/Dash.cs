using UnityEngine;

public class DashRigidbody : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 30f;   // Velocidad del dash
    public float dashDuration = 0.2f; // Duración en segundos
    public float dashCooldown = 1f; // Tiempo de recarga

    private Rigidbody rb;
    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float lastDashTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing && Time.time < dashEndTime)
        {
            rb.linearVelocity = transform.forward * dashSpeed; // Dash en la dirección de la vista
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

        rb.useGravity = false; // Desactiva la gravedad temporalmente
    }

    void EndDash()
    {
        isDashing = false;
        rb.useGravity = true; // Reactiva la gravedad

        // Opcional: Mantener un poco de velocidad después del dash
        rb.linearVelocity *= 0.5f;
    }
}

