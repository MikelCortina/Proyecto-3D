using System.Collections;
using TMPro;
using UnityEngine;

public class Impulsos : MonoBehaviour
{
    public Transform muzzle;
    public Transform muzzleBalas;
    private float fireRate = 0.1f;
    private float nextFireTime = 0f;
    public new Camera camera;
    public Rigidbody rb;
    private float shootForce = 200;
    public GameObject projectilePrefab;
    private float recoilRotationAmount = 0.1f;
    private float recoilDistance = 0.1f;
    private float recoilCameraDistance = 0.1f;
    private float recoilDuration = 0.1f;
    public PlayerMovement player;

    private Vector3 originalPosition1;
    private Vector3 originalPosition2;
    private Quaternion originalMuzzleRotation;
    private Quaternion originalWeaponRotation;   // Rotación inicial del arma
    private Quaternion reloadRotation;           // Rotación hacia arriba durante recarga
    private float recoilTimer = 0f;

    public float charger = 15;
    public float chargerMax = 15;
    public float recargaTimer = 1.5f;

    public AudioClip shootSound;
    public AudioSource audioSource;

    public TextMeshProUGUI bulletText;

    private bool isReloading = false; // Bandera para saber si estamos recargando

    public ParticleSystem speedParticles; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles2;
    public ParticleSystem bulletParticles1; // Arrastra el Particle System desde el Inspector
    public ParticleSystem bulletParticles2; // Arrastra el Particle System desde el Inspector
    public ParticleSystem bulletParticles3; // Arrastra el Particle System desde el Inspector
    public ParticleSystem bulletParticles4; // Arrastra el Particle System desde el Inspector



    void Awake()
    {
        speedParticles2.Stop();
        bulletParticles1.Stop();
        bulletParticles2.Stop();
        bulletParticles3.Stop();
        bulletParticles4.Stop();
    }
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

    }

    void Update()
    {
        

        // Disparo si se mantiene presionado el botón derecho y no se está recargando
        if (Input.GetMouseButton(1) && Time.time >= nextFireTime && charger > 0 && !isReloading)
        {
            Shoot();
            ShootBullet();
            if (player.rapido)
            {

                speedParticles.Play(); // Activa las partículas
                speedParticles2.Play();
            }
            nextFireTime = Time.time + fireRate;

        }
        if (Input.GetMouseButtonDown(1))
        {
            
        }
        if (Input.GetMouseButtonUp(1))
        {
            bulletParticles1.Stop();
            bulletParticles2.Stop();
            bulletParticles3.Stop();
            bulletParticles4.Stop();
            if (speedParticles.isPlaying)
            {
                speedParticles.Stop();
                speedParticles2.Stop();
            }
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


        DisplayBullets();
    }

    void Shoot()
    {
       bulletParticles1.Play();
       bulletParticles2.Play();
       bulletParticles3.Play();
       bulletParticles4.Play();
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
  

    void DisplayBullets()
    {
        bulletText.text = charger + "/" + chargerMax;
    }
    
}