using System.Collections;
using UnityEngine;

public class EnemyShooterSingle : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform player;
    public float detectionRadius = 10f;
    public float shootInterval = 2f;
    public float projectileSpeed = 5f;
    public float oscillationAmplitude = 0.5f;
    public float oscillationFrequency = 2f;
    public Animator animator;

    private void Start()
    {
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
                StartCoroutine(ShootBurst());
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private IEnumerator ShootBurst()
    {
        for (int i = 0; i < 3; i++)
        {
            animator.ResetTrigger("Shoot");
            animator.SetTrigger("Shoot");
            ShootAtPlayer();
            yield return new WaitForSeconds(0.15f);
        }
        animator.SetTrigger("DontShoot");
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
}
