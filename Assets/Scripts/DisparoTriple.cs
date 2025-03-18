using System.Collections;
using UnityEngine;

public class EnemyShooterSingle : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform player;
    public float detectionRadius = 10f;
    public float shootInterval = 2f;
    public float projectileSpeed = 5f;
    public Color detectionRadiusColor = Color.red;
    private Animator animator;
    public float oscillationAmplitude = 0.5f;
    public float oscillationFrequency = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("No se encontró un Animator en el objeto ni en sus hijos.");
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        StartCoroutine(ShootingLoop());
    }

    private IEnumerator ShootingLoop()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRadius)
            {
                yield return StartCoroutine(ShootSequence());
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private IEnumerator ShootSequence()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        for (int i = 0; i < 3; i++) // Disparar tres proyectiles con 0.1 segundos de diferencia
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(0.25f);
        }

        if (animator != null)
        {
            animator.SetTrigger("DontShoot");
        }

        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de comenzar de nuevo
    }

    private void ShootAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileBehavior projectileScript = projectile.GetComponent<ProjectileBehavior>();

        if (projectileScript != null)
        {
            projectileScript.MoveProjectile(direction, projectileSpeed);
            projectileScript.StartOscillation(oscillationAmplitude, oscillationFrequency);
        }

        projectile.transform.LookAt(player.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectionRadiusColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
