using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections; // Importa el espacio de nombres para TextMesh Pro

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 20f;
    private float lookSpeed = 1f;
    private float maxLookAngle = 90f;
    private float jumpForce = 1250f;
    private float inertiaFactor = 50f; // Factor de inercia
    private float maxVelocity = 40f; // Velocidad máxima permitida
    private float originSpeed;
    private float originMaxVelocity;
    public Vector3 originalVelocity;
    private float speed;



    public Rigidbody rb;
    public Camera playerCamera;
    private float rotationX = 0f;
    private bool isGrounded;

    public AudioClip[] footstepSounds;  // Array de sonidos de pasos

    public float stepInterval;    // Tiempo entre pasos
    private float stepTimer = 0f;        // Controla el tiempo entre pasos

    private int lastFootstepIndex = -1; // Guarda el último sonido que se usó

    public AudioSource audioSource;
    public AudioClip landSound;
    public bool isMovingTowards = false;
    public AudioClip jumpSound;
    public AudioClip dashSound;


    public DashRigidbody dashRigidbody;
    // Referencia al componente TextMesh Pro para mostrar la velocidad
    public TextMeshProUGUI speedText;  // Usa TextMeshProUGUI

    private bool wasGrounded;
    private float landSoundCooldown = 1f;  // Duración del cooldown en segundos
    private float landSoundCooldownTimer = 0f; // Contador

    public bool rapido = false;

    public ParticleSystem speedParticles; // Arrastra el Particle System desde el Inspector

    private Coroutine currentDashEffectCoroutine; // Guarda la corutina actual en ejecución






    void Awake()
    {

        speedParticles.Stop();
        // Busca todos los textos en la escena
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI tmp in allTexts)
        {
            if (tmp.text == "Speed") // Aquí pones el texto que quieres buscar
            {
                speedText = tmp;
                Debug.Log("speedText asignado automáticamente a: " + tmp.gameObject.name);
                break;
            }
        }

        if (speedText == null)
        {
            Debug.LogWarning("No se encontró un TextMeshProUGUI con el texto 'Speed'");
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();


        Cursor.lockState = CursorLockMode.Locked; // Para que el cursor no se vea.
        Cursor.visible = false; // Hace invisible el cursor.
        rb.freezeRotation = true;

        originMaxVelocity = maxVelocity;
        originSpeed = moveSpeed;
    }

    void Update()
    {
        if (landSoundCooldownTimer > 0f)
        {
            landSoundCooldownTimer -= Time.deltaTime;
        }
        if (rb.linearVelocity.y < 0) // Solo cuando cae
        {
            rb.AddForce(Vector3.down * 1.5f, ForceMode.Acceleration); // Aumenta la gravedad
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(Vector3.down * 5000f, ForceMode.Impulse);

        }
        MovePlayer();

        CheckGrounded();

        PlayFootsteps();


        // Solo saltas si el cooldown terminó
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                audioSource.PlayOneShot(jumpSound);
                Jump();

            }
        }
        if (!wasGrounded && isGrounded)
        {
            if (landSoundCooldownTimer <= 0f)
            {
                if (landSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(landSound);
                }

                // Reiniciar el cooldown después de reproducir el sonido
                landSoundCooldownTimer = landSoundCooldown;
            }
        }

        wasGrounded = isGrounded;


        DisplaySpeed();
        LookAround();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("ZonaVelocidad"))
        {
            maxVelocity = 50f;
            moveSpeed = 35f;
            stepInterval = 0.2f;

            speedParticles.Play(); // Activa las partículas

        
        }
        else
        {

            maxVelocity = originMaxVelocity;
            moveSpeed = originSpeed;
            stepInterval = 0.35f;

        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ZonaVelocidad"))
        {
            maxVelocity = originMaxVelocity;
            moveSpeed = originSpeed;

            if (!isMovingTowards)
            {
                speedParticles.Stop(); // Activa las partículas
            }

        }
    }

    void MovePlayer()
    {
        float speed = moveSpeed;

        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Normalizar la dirección de movimiento para evitar el aumento de velocidad en diagonal
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Aplicar inercia en el movimiento solo en los ejes X y Z
        Vector3 targetVelocity = moveDirection * speed;
        Vector3 velocityXZ = Vector3.Lerp(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z), targetVelocity, Time.deltaTime * inertiaFactor);

        // Limitar la velocidad en los ejes X y Z sin afectar la caída en Y
        if (velocityXZ.magnitude > maxVelocity)
        {
            velocityXZ = velocityXZ.normalized * maxVelocity;
        }

        // Aplicar la nueva velocidad manteniendo el valor de Y sin cambios
        rb.linearVelocity = new Vector3(velocityXZ.x, rb.linearVelocity.y, velocityXZ.z);

        float currentSpeed = rb.linearVelocity.magnitude;
        float verticalSpeed = rb.linearVelocity.y;

        // Verifica si la velocidad total supera el umbral o si la velocidad en Y es mayor a 20
        if (verticalSpeed > 9f)
        {
            rapido = true;
        }

        else
        {
            rapido = false;
        }
    }



    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void CheckGrounded()
    {
        RaycastHit hit;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.6f); // Ajusta el valor 1.1f según el tamaño del jugador

    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Método para mostrar la velocidad en pantalla
    void DisplaySpeed()
    {
        // Mostrar la velocidad actual del jugador en el texto de TextMeshPro
        speedText.text = "Speed: " + rb.linearVelocity.magnitude.ToString("F2") + " m/s";
    }
    public void MoveToEnemy(Vector3 enemyPosition)
    {
        audioSource.PlayOneShot(dashSound);
        StartCoroutine(MoveToPositionCoroutine(enemyPosition));
    }
    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition)
    {

        originalVelocity = rb.linearVelocity;

        float journeyLength = Vector3.Distance(transform.position, targetPosition);
        float startTime = Time.time;
        speedParticles.Play(); // Activa las partículas

        // Lerp desde la posición actual hasta la del enemigo
        while (Vector3.Distance(transform.position, targetPosition) > 2f) // Menor tolerancia
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            fractionOfJourney = Mathf.Clamp01(fractionOfJourney); // Aseguramos que no se pase del 100%

            transform.position = Vector3.Lerp(transform.position, targetPosition, fractionOfJourney);



            yield return null;
        }

        // Aseguramos que el jugador llegue exactamente a la posición del enemigo
        transform.position = targetPosition;

        speedParticles.Stop(); // Activa las partículas
        // Restauramos la velocidad original
        rb.linearVelocity = originalVelocity;

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Glovo"))
        {
            // Si hay una corutina corriendo, la paramos antes de iniciar otra
            if (currentDashEffectCoroutine != null)
            {
                StopCoroutine(currentDashEffectCoroutine);
            }

            currentDashEffectCoroutine = StartCoroutine(DashEffect());

            // Lógica del impulso vertical para Glovo
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 25f, rb.linearVelocity.z);
            dashRigidbody.canDash = true;
            dashRigidbody.isDashing = false;
            rb.useGravity = true;

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("SuperGlovo"))
        {
            if (currentDashEffectCoroutine != null)
            {
                StopCoroutine(currentDashEffectCoroutine);
            }

            currentDashEffectCoroutine = StartCoroutine(DashEffect());

            // Lógica del impulso vertical para SuperGlovo
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 100f, rb.linearVelocity.z);
            dashRigidbody.canDash = true;
            dashRigidbody.isDashing = false;
            rb.useGravity = true;

            Destroy(collision.gameObject);
        }
    }
    void PlayFootsteps()
    {
        float speed = rb.linearVelocity.magnitude;

        // Condiciones para que suenen los pasos:
        if (isGrounded && speed > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                if (footstepSounds.Length > 0 && audioSource != null)
                {
                    int index;

                    // Escoger un índice al azar, pero distinto al anterior
                    do
                    {
                        index = Random.Range(0, footstepSounds.Length);
                    } while (index == lastFootstepIndex && footstepSounds.Length > 1);

                    // Reproducir el sonido elegido
                    audioSource.PlayOneShot(footstepSounds[index]);

                    // Guardamos el último sonido para evitar repetidos
                    lastFootstepIndex = index;

                    // Reiniciar el temporizador del paso
                    stepTimer = stepInterval;
                }
            }
        }
        else
        {
            // Si no se mueve o no está en el suelo, se resetea el temporizador
            stepTimer = 0f;
        }
    }


    IEnumerator DashEffect()
    {
        speedParticles.Play();
        yield return new WaitForSeconds(0.5f);
        speedParticles.Stop();
    }
}

