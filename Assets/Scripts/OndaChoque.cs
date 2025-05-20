using System.Collections;
using UnityEngine;

public class PlayerFallAttack : MonoBehaviour
{
    private float fallForce = 10000f;  // Fuerza de ca�da
    public float damageRadius = 5f;  // Radio de da�o
    public string enemyTag = "Enemy"; // Tag de los enemigos

    private Rigidbody rb;
    private bool isFalling = false;
    private DashRigidbody dashRigidbody;
    private Impulsos impulsos;
    public Animator HUDanimator;
    public Animator animator;
    public DashRigidbody dash;

    public ParticleSystem speedParticles; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles2; // Arrastra el Particle System desde el Inspector

    public bool canFall = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashRigidbody = GetComponent<DashRigidbody>();
        impulsos = GetComponent<Impulsos>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canFall)
        {
            StartFallAttack();
        }
    }
    public void StartFallAttack()
    {
        StartCoroutine(FallAnim());
        HUDanimator.ResetTrigger("CanPush"); // Evita que se superpongan triggers
        HUDanimator.SetTrigger("CantPush");
        canFall = false;
        rb.AddForce(Vector3.down * fallForce, ForceMode.Impulse);
        isFalling = true; // Marca que el jugador est� cayendo
        speedParticles.Play();
        speedParticles2.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            speedParticles.Stop();
            speedParticles2.Stop();
            // Detectar todos los objetos en el radio de da�o
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach (Collider hit in hitColliders)
            {
                // Verificar si el objeto tiene la tag de enemigo
                if (hit.CompareTag(enemyTag))
                {
                    HUDanimator.ResetTrigger("CantPush"); // Evita que se superpongan triggers
                    HUDanimator.SetTrigger("CanPush");
                    dash.HUDanimator.ResetTrigger("CantDash");
                    dash.HUDanimator.SetTrigger("CanDash");
                    Destroy(hit.gameObject); // Elimina al enemigo
                    dashRigidbody.canDash = true;
                    impulsos.charger = impulsos.chargerMax;
                }
            }

            isFalling = false; // Reinicia la variable
        }
    }

    void OnDrawGizmos()
    {
        // Dibuja la esfera de da�o en la escena para depuraci�n
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    public IEnumerator FallAnim()
    {
        animator.speed = 4f;
        if (animator != null)
        {
            animator.SetTrigger("IsFalling");
        }

        yield return new WaitForSeconds(0.25f);

        if (animator != null)
        {
            animator.SetTrigger("IsntFalling");
            Debug.Log("Cambio a IdleShooting");
        }

        animator.speed = 1f;
    }

}