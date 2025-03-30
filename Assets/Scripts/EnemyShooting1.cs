using System.Collections;
using UnityEngine;


public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform player;
    public float detectionRadius = 10f;
    public LayerMask obstacleLayer; // Capa para los obstáculos que deben bloquear la visión.

    [Header("Disparo")]
    public float minShootInterval = 1f; // Intervalo mínimo entre disparos
    public float maxShootInterval = 3f; // Intervalo máximo entre disparos
    public float projectileSpeed = 5f;

    [Header("Debug Gizmo")]
    public Color detectionRadiusColor = Color.red;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            Debug.Log("Animator encontrado y asignado.");
        }
        else
        {
            Debug.LogWarning("No se encontró un Animator en el objeto ni en sus hijos.");
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Empezamos el ciclo de disparos con un delay aleatorio
        float randomStartDelay = Random.Range(0f, 2f);
        StartCoroutine(ShootingLoop(randomStartDelay));
    }

    private IEnumerator ShootingLoop(float initialDelay)
    {
        // Espera inicial aleatoria para desincronizar los enemigos al empezar
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                // Verifica si hay algún obstáculo entre el enemigo y el jugador
                if (!IsObstacleBetweenPlayerAndEnemy())
                {
                    StartCoroutine(ShootAnim());
                }
            }

            // Espera entre disparos con un intervalo aleatorio
            float randomShootInterval = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(randomShootInterval);
        }
    }

    private bool IsObstacleBetweenPlayerAndEnemy()
    {
        // Disparar un rayo desde el enemigo hacia el jugador para verificar obstáculos
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, obstacleLayer))
        {
            // Si el rayo golpea un objeto en la capa de obstáculos, significa que hay un obstáculo
            return true; // Hay un obstáculo
        }

        return false; // No hay obstáculo
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

    IEnumerator ShootAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shoot");            
        }

        yield return new WaitForSeconds(0.2f);

        ShootAtPlayer();

        if (animator != null)
        {
            animator.SetTrigger("DontShoot");
        }
    }
}
