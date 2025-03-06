using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefabricado del proyectil
    public Transform muzzle;  // La posición de la boca del arma (donde se dispara)
    public float fireRate = 0.5f;  // Tiempo entre disparos
    private float nextFireTime = 0f;  // Para controlar el tiempo de recarga entre disparos
    public new Camera camera;
    private Vector3 originalPosition1; // Posición original de la cámara
    private Vector3 originalPosition2; // Posición original de la boca del arma
    public float recoilAmount = 10f; // Ángulo de retroceso en grados
    public float recoilDuration = 0.1f; // Duración del retroceso
    private float recoilTimer = 0f; // Temporizador de retroceso
    private Quaternion originalCameraRotation; // Rotación original de la cámara
    private Quaternion originalMuzzleRotation; // Rotación original del muzzle

    private void Start()
    {
        // Guardamos la posición y rotación originales de la cámara y el arma
        originalPosition1 = camera.transform.localPosition;

        originalMuzzleRotation = muzzle.transform.localRotation;
    }

    void Update()
    {
        // Detectar si el jugador hace clic para disparar
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) // "Fire1" es el clic izquierdo por defecto
        {
            Shoot();
            nextFireTime = Time.time + fireRate;  // Controlar la tasa de disparo
        }

        // Controla la duración del retroceso
        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime;

            // Rotar la cámara hacia arriba rápidamente


            // Rotar la boca del arma (muzzle) hacia arriba rápidamente
            muzzle.transform.localRotation = Quaternion.Slerp(originalMuzzleRotation, originalMuzzleRotation * Quaternion.Euler(-recoilAmount, 0, 0), (1 - (recoilTimer / recoilDuration)));
        }
        else
        {
            // Restablecer la rotación de la cámara y el arma

            muzzle.transform.localRotation = originalMuzzleRotation;
        }
    }

    void Shoot()
    {
        // Obtener la dirección hacia donde está mirando la cámara
        Vector3 shootingDirection = camera.transform.forward;

        // Instanciar el proyectil
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.LookRotation(shootingDirection));
        recoilTimer = recoilDuration;  // Iniciar el retroceso
    }
}