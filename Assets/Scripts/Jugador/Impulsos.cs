using System.Collections;
using TMPro;
using UnityEngine;

public class Impulsos : MonoBehaviour
{
    public Transform muzzle;
    public Transform muzzleBalas;
    public float fireRate;
    private float nextFireTime = 0f;
    public new Camera camera;
    public Rigidbody rb;
    public float shootForce;
    public GameObject projectilePrefab;
    public float recoilRotationAmount;
    public float recoilDistance;
    public float recoilCameraDistance = 0.1f;
    public float recoilDuration = 0.1f;

    private Vector3 originalPosition1;
    private Vector3 originalPosition2;
    private Quaternion originalMuzzleRotation;
    private Quaternion originalWeaponRotation;   // Rotación inicial del arma
    private Quaternion reloadRotation;           // Rotación hacia arriba durante recarga
    private float recoilTimer = 0f;

    public float charger;
    public float chargerMax;
    public float recargaTimer;

    public AudioClip shootSound;
    public AudioSource audioSource;

    public TextMeshProUGUI bulletText;

    private bool isReloading = false; // Bandera para saber si estamos recargando

    public float grappleSpeed = 20f;
    public float maxGrappleDistance = 50f;
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private bool isHanging = false;

    private LineRenderer lineRenderer; // Línea del gancho

    void Start()
    {
        originalPosition1 = camera.transform.localPosition;
        originalPosition2 = muzzle.transform.localPosition;
        originalMuzzleRotation = muzzle.transform.localRotation;

        // Guardamos la rotación original del arma
        originalWeaponRotation = muzzle.transform.localRotation;

        // Rotación de recarga: 75 grados en el eje Y
        reloadRotation = originalWeaponRotation * Quaternion.Euler(0f, 0f, -75f);

        chargerMax = charger;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;   // Grosor de la línea al inicio
        lineRenderer.endWidth = 0.05f;     // Grosor de la línea al final
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Material simple
        lineRenderer.startColor = Color.cyan;  // Color de la línea
        lineRenderer.endColor = Color.cyan;
        lineRenderer.enabled = false; // Ocultar al inicio
    }

    void Update()
    {
        

        // Disparo si se mantiene presionado el botón derecho y no se está recargando
        if (Input.GetMouseButton(1) && Time.time >= nextFireTime && charger > 0 && !isReloading)
        {
            Shoot();
            ShootBullet();
            nextFireTime = Time.time + fireRate;
        }

        // Recargar manualmente con la tecla R
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && charger < chargerMax)
        {
            StartCoroutine(Reload());
        }

        // Efecto de retroceso
        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime;
            camera.transform.localPosition = Vector3.Lerp(originalPosition1, originalPosition1 - new Vector3(0, 0, recoilCameraDistance), (1 - (recoilTimer / recoilDuration)));
            muzzle.transform.localPosition = Vector3.Lerp(originalPosition2, originalPosition2 - new Vector3(0, 0, recoilDistance), (1 - (recoilTimer / recoilDuration)));

            float randomRecoilX = Random.Range(-recoilRotationAmount, recoilRotationAmount);
            float randomRecoilY = Random.Range(-recoilRotationAmount, recoilRotationAmount);
            Quaternion baseRotation = Quaternion.Euler(0, 0, 0);
            Quaternion targetRotation = Quaternion.Euler(randomRecoilX, randomRecoilY, 0) * baseRotation * originalMuzzleRotation;
            muzzle.transform.localRotation = Quaternion.Slerp(originalMuzzleRotation, targetRotation, 1 - (recoilTimer / recoilDuration));
        }
        else if (!isReloading) // Solo resetea cuando no se está recargando
        {
            camera.transform.localPosition = originalPosition1;
            muzzle.transform.localPosition = originalPosition2;
            muzzle.transform.localRotation = originalMuzzleRotation;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryGrapple();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isGrappling = false;
            isHanging = false;
            lineRenderer.enabled = false; // Ocultar línea
        }

        if (isGrappling)
        {
            MoveTowardsGrapplePoint();
            UpdateGrappleLine();
            RestrictDistanceAndSpeed(); // Restringir movimiento
        }

        DisplayBullets();
    }

    void Shoot()
    {
        Vector3 shootingDirection = -camera.transform.forward;
        rb.AddForce(shootingDirection * shootForce, ForceMode.Impulse);
        recoilTimer = recoilDuration;
        charger--;
    }

    void ShootBullet()
    {
        Vector3 shootingDirection = camera.transform.forward;
        GameObject projectile = Instantiate(projectilePrefab, muzzleBalas.position, Quaternion.LookRotation(shootingDirection));

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    void TryGrapple()
    {
        RaycastHit hit;
        Vector3 rayOrigin = camera.transform.position;
        Vector3 rayDirection = camera.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxGrappleDistance))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            isHanging = false;

            // Activar línea del gancho
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, muzzle.position); // Inicio en el arma
            lineRenderer.SetPosition(1, grapplePoint);    // Fin en el punto de impacto
        }
    }
    void MoveTowardsGrapplePoint()
    {
        if (Vector3.Distance(transform.position, grapplePoint) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, grapplePoint, grappleSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, grapplePoint) <= 1f)
        {
            isGrappling = false;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            isHanging = true;
        }
        else
        {
            isGrappling = false;
            isHanging = false;
            lineRenderer.enabled = false; // Ocultar línea al soltar
        }
    }
    void UpdateGrappleLine()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, muzzle.position); // Actualizar inicio en el arma
            lineRenderer.SetPosition(1, grapplePoint);    // Mantener punto final en el gancho
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recargando...");

        float elapsedTime = 0f;

        // Rotación hacia la posición de recarga durante el tiempo de recarga
        while (elapsedTime < recargaTimer)
        {
            float t = elapsedTime / recargaTimer;
            muzzle.transform.localRotation = Quaternion.Slerp(originalWeaponRotation, reloadRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que termine en la rotación de recarga exacta
        muzzle.transform.localRotation = reloadRotation;

        // Esperar un momento si quieres (opcional)
        // yield return new WaitForSeconds(0.1f);

        // Regreso suave a la rotación original
        float returnTime = 0.3f; // Tiempo que tarda en volver a la posición original
        elapsedTime = 0f;

        while (elapsedTime < returnTime)
        {
            float t = elapsedTime / returnTime;
            muzzle.transform.localRotation = Quaternion.Slerp(reloadRotation, originalWeaponRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que se quede exactamente en la rotación original
        muzzle.transform.localRotation = originalWeaponRotation;

        charger = chargerMax;
        isReloading = false;

        Debug.Log("Recarga completa.");
    }
    void RestrictDistanceAndSpeed()
    {
        if (isGrappling && Input.GetKey(KeyCode.LeftShift))
        {
            float currentDistance = Vector3.Distance(transform.position, grapplePoint);

            // Si está más lejos que la distancia máxima, limitar la posición
            if (currentDistance > maxGrappleDistance)
            {
                Vector3 directionToGrapple = (grapplePoint - transform.position).normalized;
                transform.position = grapplePoint - (directionToGrapple * maxGrappleDistance);

                // Reducir la velocidad si intenta alejarse demasiado
                rb.linearVelocity *= 0.8f; // Disminuye progresivamente la velocidad
            }

            // Si está cerca del punto y sigue manteniendo Shift, suspenderlo en el aire
            if (currentDistance < 1.5f)
            {
                isHanging = true;
                rb.linearVelocity = Vector3.zero; // Detener el movimiento al colgarse
            }
            else
            {
                isHanging = false;
            }
        }
    }

    void DisplayBullets()
    {
        bulletText.text = charger + "/" + chargerMax;
    }
}