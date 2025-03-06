using UnityEngine;

public class Impulsos : MonoBehaviour
{
    public Transform muzzle;
    public float fireRate;  // Tiempo entre disparos ajustado a 0.25 segundos
    private float nextFireTime = 0f;  // Para controlar el tiempo de recarga entre disparos
    public new Camera camera;
    public Rigidbody rb;
    public float shootForce;
    public GameObject projectilePrefab;

    public float recoilDistance = 0.1f; // Distancia de retroceso de la cámara
    public float recoilDuration = 0.1f; // Duración del retroceso
    private Vector3 originalPosition1; // Posición original de la cámara
    private Vector3 originalPosition2;
    private float recoilTimer = 0f; // Temporizador de retroceso

    void Start()
    {
        // Guardamos la posición original de la cámara
        originalPosition1 = camera.transform.localPosition;
        originalPosition2 = muzzle.transform.localPosition;

    }

    void Update()
    {
        if (Input.GetMouseButton(1) && Time.time >= nextFireTime)
        {
            Shoot();
            ShootBullet();
            nextFireTime = Time.time + fireRate; // Actualiza el tiempo del próximo disparo
        }

        // Controla la duración del retroceso
        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime;
            // Mover la cámara hacia atrás en el eje Z
            camera.transform.localPosition = Vector3.Lerp(originalPosition1, originalPosition1 - new Vector3(0, 0, recoilDistance), (1 - (recoilTimer / recoilDuration)));
            muzzle.transform.localPosition = Vector3.Lerp(originalPosition2, originalPosition2 - new Vector3(0, 0, recoilDistance), (1 - (recoilTimer / recoilDuration)));
        }
        else
        {
            // Restablecer la posición de la cámara a la original
            camera.transform.localPosition = originalPosition1;
            muzzle.transform.localPosition = originalPosition2;
        }
    }

    void Shoot()
    {
        // Obtener la dirección hacia donde está mirando la cámara
        Vector3 shootingDirection = -camera.transform.forward;
        rb.AddForce(shootingDirection * shootForce, ForceMode.Impulse);

        // Activar el retroceso de la cámara
        recoilTimer = recoilDuration;
    }
    void ShootBullet()
    {
        // Obtener la dirección hacia donde está mirando la cámara
        Vector3 shootingDirection = camera.transform.forward;

        // Instanciar el proyectil
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.LookRotation(shootingDirection));


    }
}
