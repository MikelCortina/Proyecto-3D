using UnityEngine;

public class PlayerFallAttack : MonoBehaviour
{
    public float fallForce = 5000f;  // Fuerza de caída
    public float damageRadius = 5f;  // Radio de daño
    public string enemyTag = "Enemy"; // Tag de los enemigos

    private Rigidbody rb;
    private bool isFalling = false;
    private DashRigidbody dashRigidbody;
    private Impulsos impulsos;

    public ParticleSystem speedParticles; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles2; // Arrastra el Particle System desde el Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashRigidbody = GetComponent<DashRigidbody>();
        impulsos = GetComponent<Impulsos>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(Vector3.down * fallForce, ForceMode.Impulse);
            isFalling = true; // Marca que el jugador está cayendo
            speedParticles.Play();
            speedParticles2.Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            speedParticles.Stop();
            // Detectar todos los objetos en el radio de daño
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach (Collider hit in hitColliders)
            {
                // Verificar si el objeto tiene la tag de enemigo
                if (hit.CompareTag(enemyTag))
                {
                   
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
        // Dibuja la esfera de daño en la escena para depuración
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
