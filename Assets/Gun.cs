using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefabricado del proyectil
    public Transform muzzle;  // La posición de la boca del arma (donde se dispara)
    public float fireRate = 0.5f;  // Tiempo entre disparos
    private float nextFireTime = 0f;  // Para controlar el tiempo de recarga entre disparos

    void Update()
    {
        // Detectar si el jugador hace clic para disparar
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) // "Fire1" es el clic izquierdo por defecto
        {
            Shoot();
            nextFireTime = Time.time + fireRate;  // Controlar la tasa de disparo
        }
    }

    void Shoot()
    {
        // Obtener la dirección hacia donde está mirando la cámara
        Vector3 shootingDirection = Camera.main.transform.forward;

        // Instanciar el proyectil
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.LookRotation(shootingDirection));

        // Obtener el Rigidbody del proyectil y darle una velocidad
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootingDirection * 10f;  // Ajusta la velocidad como sea necesario
        }
    }

}
