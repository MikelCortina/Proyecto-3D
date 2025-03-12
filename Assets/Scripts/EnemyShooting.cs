using System.Security.Cryptography;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefab del proyectil
    public Transform player;  // Referencia al jugador
    public float detectionRadius = 10f;  // Distancia a la que el enemigo detecta al jugador
    public float shootInterval = 2f;  // Intervalo entre disparos
    public float projectileSpeed = 5f;  // Velocidad del proyectil ajustada
    public Color detectionRadiusColor = Color.red;  // Color del Gizmo
    public Impulsos armaJugador;
    public DashRigidbody dash;

    private void Start()
    {
        // Si no se asigna el jugador en el inspector, se busca automáticamente.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Empieza la coroutine de disparo
        InvokeRepeating("TryShoot", 0f, shootInterval);
    }

    private void TryShoot()
    {
        // Verifica si el jugador está dentro del radio de detección
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        // Instanciar el proyectil
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calcular la dirección del proyectil hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Mover el proyectil manualmente usando Translate (sin Rigidbody)
        ProjectileBehavior projectileScript = projectile.GetComponent<ProjectileBehavior>();
        if (projectileScript != null)
        {
            projectileScript.MoveProjectile(direction, projectileSpeed);
        }

        // También podemos rotar el proyectil para que apunte hacia el jugador visualmente
        projectile.transform.LookAt(player.position);  // Asegura que el proyectil mire hacia el jugador
    }

    // Método para dibujar el radio de detección en el Editor
    private void OnDrawGizmos()
    {
        // Si la variable detectionRadius no es 0, dibujamos el Gizmo
        Gizmos.color = detectionRadiusColor;  // Establecer el color del Gizmo
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // Dibujar una esfera al rededor del enemigo
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bala"))
        {
            
            Debug.Log("¡Impacto registrado!"); // Para verificar
            Destroy(gameObject, 0.1f); // Pequeño delay para asegurar que la colisión se procesa
           
        }
    }
    private void OnDestroy()
    {
        armaJugador.charger = armaJugador.chargerMax;
        dash.lastDashTime = -999f;
    }

}

