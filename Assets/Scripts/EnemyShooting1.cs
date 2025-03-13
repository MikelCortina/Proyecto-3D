using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefab del proyectil
    public Transform player;  // Referencia al jugador
    public float detectionRadius = 10f;  // Distancia a la que el enemigo detecta al jugador
    public float shootInterval = 2f;  // Intervalo entre disparos
    public float projectileSpeed = 5f;  // Velocidad del proyectil ajustada
    public Color detectionRadiusColor = Color.red;  // Color del Gizmo

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Empieza la coroutine de disparo
        InvokeRepeating("TryShoot", 0f, shootInterval);
    }

    private void TryShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius)
        {
            ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 direction = (player.position - transform.position).normalized;

        ProjectileBehavior projectileScript = projectile.GetComponent<ProjectileBehavior>();
        if (projectileScript != null)
        {
            projectileScript.MoveProjectile(direction, projectileSpeed);
        }

        projectile.transform.LookAt(player.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectionRadiusColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
