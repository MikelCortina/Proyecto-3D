using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class DashRigidbody : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Ground Check Settings")]
    public float groundCheckDistance = 1.1f;

    private Rigidbody rb;
    private Collider playerCollider;    // <<<< Referencia al collider del jugador
    private Vector3 originalColliderSize; // <<<< Guarda el tamaño original (para BoxCollider)
    private Vector3 dashColliderSize = new Vector3(3f, 3f, 3f); // <<<< Tamaño durante el dash (ajústalo según tu juego)

    public bool isDashing = false;
    public float dashEndTime = 0f;
    public float lastDashTime = -999f;
    public bool canDash = true;
    public bool hasDashed;


    public ParticleSystem speedParticles; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles2; // Arrastra el Particle System desde el Inspector

    public float speedThreshold; // Velocidad mínima para activar partículas

    public Animator animator;

    public AudioClip dashSound;
    public AudioSource audioSource;

    public LevelManager levelManager;



    void Start()
    {
        speedParticles.Stop();
        speedParticles2.Stop();

        rb = GetComponent<Rigidbody>();
       
    }

    void Update()
    {
        if (levelManager.playable == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && canDash)
            {
                audioSource.PlayOneShot(dashSound);
                StartDash();
            }
        }      
    }

    void FixedUpdate()
    {
        if (isDashing && Time.time < dashEndTime)
        {
            rb.linearVelocity = transform.forward * dashSpeed;

            // Lanza un SphereCast al frente mientras dasheas
            DetectEnemiesInDash();


            // Activa las partículas

        }
        else if (isDashing)
        {
            StartCoroutine(EndDash());


        }
    }

    void DetectEnemiesInDash()
    {
        float sphereRadius = 1f;  // ajusta el radio a tu necesidad
        float rayLength = 2f;     // distancia al frente para detectar enemigos

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, transform.forward, rayLength);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemigo"))
            {
                hit.collider.gameObject.GetComponent<EnemyHealth>().IniciarDisolucion();
                canDash = true;  // Si quieres permitir concatenar el dash tras destruir uno
            }
        }
    }

    void StartDash()
    {
        StartCoroutine(DashAnim());
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        lastDashTime = Time.time;
        canDash = false;
        hasDashed = true;
        speedParticles.Play();
        speedParticles2.Play();


        rb.useGravity = false;

        // Aumenta el tamaño del collider durante el dash
        if (playerCollider is BoxCollider)
        {
            ((BoxCollider)playerCollider).size = dashColliderSize;
        }
        else if (playerCollider is CapsuleCollider)
        {
            ((CapsuleCollider)playerCollider).height = dashColliderSize.y;
            ((CapsuleCollider)playerCollider).radius = dashColliderSize.x; // usa X o Z según prefieras
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDashing && other.CompareTag("Enemigo"))
        {
            canDash = true;
        }
    }

    IEnumerator EndDash()
    {
        isDashing = false;
        rb.useGravity = true;
        speedParticles.Stop();
        speedParticles2.Stop();
        // Espera un poquito antes de terminar el dash (opcional)
        yield return new WaitForSeconds(0.15f);

        hasDashed = false;
        rb.linearVelocity *= 0.5f;

        // Regresa el tamaño del collider a su estado original
        if (playerCollider is BoxCollider)
        {
            ((BoxCollider)playerCollider).size = originalColliderSize;
        }
        else if (playerCollider is CapsuleCollider)
        {
            ((CapsuleCollider)playerCollider).height = originalColliderSize.y;
            ((CapsuleCollider)playerCollider).radius = originalColliderSize.x;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    public IEnumerator DashAnim()
    {
      
            animator.speed = 5f;
            if (animator != null)
            {
                animator.Play("Dash2", 0, 0f);         // Empieza desde el principio sí o sí
            }

            // Espera el tiempo necesario para el dash o la duración de la animación
            yield return new WaitForSeconds(dashDuration / 2f);

            if (animator != null)
            {
                animator.SetTrigger("DontDash");      // Transición a otro estado si es necesario
            }
        animator.speed = 1f;
    }


}
