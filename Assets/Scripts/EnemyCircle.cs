using UnityEngine;

public class EnemyShooterBurst : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefab del proyectil
    public Transform player;  // Referencia al jugador
    public float detectionRadius;  // Distancia de detección
    public float shootInterval = 2f;  // Intervalo entre ráfagas
    public float projectileSpeed = 5f;  // Velocidad de los proyectiles
    public int projectileCount = 15;  // Cantidad de proyectiles en la ráfaga
    public Color detectionRadiusColor = Color.red;  // Color del Gizmo

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Inicia la detección en intervalos
        InvokeRepeating("TryShoot", 0f, shootInterval);
    }

    private void TryShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius)
        {
            ShootBurst();
        }
    }

    private void ShootBurst()
    {
        float angleStep = 360f / projectileCount; // Ángulo entre cada proyectil

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            ProjectileBehavior projectileScript = projectile.GetComponent<ProjectileBehavior>();
            if (projectileScript != null)
            {
                projectileScript.MoveProjectile(direction, projectileSpeed);
            }

            projectile.transform.LookAt(transform.position + direction);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectionRadiusColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
