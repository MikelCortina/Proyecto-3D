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
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
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
