using System.Collections;
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

    private Animator animator; // <-- Referencia al Animator

    private void Start()
    {

        // Asigna el Animator desde el objeto o sus hijos
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            animator.speed = 1f;
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

        // Inicia la detección en intervalos
        InvokeRepeating("TryShoot", 0f, shootInterval);
    }

    private void TryShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            StartCoroutine(ShootAnim());

        }
    }

    private void ShootBurst()
    {
        float angleStep = 360f / projectileCount;

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
    IEnumerator ShootAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shoot"); // Asegúrate que este trigger existe en tu Animator Controller
        }
        yield return new WaitForSeconds(0.2f);
        ShootBurst();

        if (animator != null)
        {
            animator.SetTrigger("DontShoot"); // Asegúrate que este trigger existe en tu Animator Controller
        }
    }
}