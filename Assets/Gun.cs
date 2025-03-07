using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefabricado del proyectil
    public Transform muzzle;  // La posición de la boca del arma (donde se dispara)
    public float fireRate = 0.5f;  // Tiempo entre disparos
    private float nextFireTime = 0f;  // Para controlar el tiempo de recarga entre disparos
    public new Camera camera;
    public float recoilAmount = 10f; // Ángulo de retroceso en grados
    public float recoilCameraAmount = 3f;
    public float recoilDuration = 0.1f; // Duración del retroceso
    private float recoilTimer = 0f; // Temporizador de retroceso
    private Quaternion originalCameraRotation; // Rotación original de la cámara
    private Quaternion originalMuzzleRotation; // Rotación original del muzzle

    private void Start()
    {

        

        originalMuzzleRotation = muzzle.transform.localRotation;
    }

    void Update()
    {
        originalCameraRotation = camera.transform.localRotation;
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
            muzzle.transform.localRotation = Quaternion.Slerp(originalMuzzleRotation, Quaternion.Euler(-recoilAmount, 0, 0) * originalMuzzleRotation, 1 - ((recoilTimer / recoilDuration) / 3));
            camera.transform.localRotation = Quaternion.Slerp(originalCameraRotation, Quaternion.Euler(-recoilCameraAmount, 0, 0) * originalCameraRotation, 1 - ((recoilTimer / recoilDuration) / 3));
            muzzle.transform.localRotation = Quaternion.Slerp(muzzle.transform.localRotation, originalMuzzleRotation, 1 - ((recoilTimer / recoilDuration) / 3 * 2));            
            camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, originalCameraRotation, 1 - ((recoilTimer / recoilDuration) / 3 * 2));
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