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
    private float recoilTimer = 0f;

    public AudioClip shootSound; // Clip de sonido del disparo
    public AudioSource audioSource; // Fuente de audio

    void Start()
    {
        originalPosition1 = camera.transform.localPosition;
        originalPosition2 = muzzle.transform.localPosition;
        originalMuzzleRotation = muzzle.transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButton(1) && Time.time >= nextFireTime)
        {
            Shoot();
            ShootBullet();
            nextFireTime = Time.time + fireRate;
        }

        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime;
            camera.transform.localPosition = Vector3.Lerp(originalPosition1, originalPosition1 - new Vector3(0, 0, recoilCameraDistance), (1 - (recoilTimer / recoilDuration)));
            muzzle.transform.localPosition = Vector3.Lerp(originalPosition2, originalPosition2 - new Vector3(0, 0, recoilDistance), (1 - (recoilTimer / recoilDuration)));

            float randomRecoilX = Random.Range(-recoilRotationAmount, recoilRotationAmount);
            float randomRecoilY = Random.Range(-recoilRotationAmount, recoilRotationAmount);
            Quaternion baseRotation = Quaternion.Euler(0, 0, 0);
            Quaternion targetRotation = Quaternion.Euler(randomRecoilX, randomRecoilY, 0) * baseRotation * originalMuzzleRotation;
            muzzle.transform.localRotation = Quaternion.Slerp(originalMuzzleRotation, targetRotation, 1 - ((recoilDuration)));
        }
        else
        {
            camera.transform.localPosition = originalPosition1;
            muzzle.transform.localPosition = originalPosition2;
            muzzle.transform.localRotation = originalMuzzleRotation;
        }
    }

    void Shoot()
    {
        Vector3 shootingDirection = -camera.transform.forward;
        rb.AddForce(shootingDirection * shootForce, ForceMode.Impulse);
        recoilTimer = recoilDuration;
    }

    void ShootBullet()
    {
        Vector3 shootingDirection = camera.transform.forward;
        GameObject projectile = Instantiate(projectilePrefab, muzzleBalas.position, Quaternion.LookRotation(shootingDirection));

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound); // Reproduce el sonido del disparo
        }
    }
}
