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
    public float fuerzaGlovo;
    public PlayerFallAttack fall;


    public Rigidbody rb;
    public Camera playerCamera;
    private float rotationX = 0f;
    private bool isGrounded;

      // Array de sonidos de pasos

    public float stepInterval;    // Tiempo entre pasos
    private float stepTimer = 0f;        // Controla el tiempo entre pasos

    private int lastFootstepIndex = -1; // Guarda el último sonido que se usó

    public AudioSource audioSource;
    public AudioClip landSound;
    public bool isMovingTowards = false;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip[] footstepSounds;


    public DashRigidbody dashRigidbody;
    // Referencia al componente TextMesh Pro para mostrar la velocidad
    public TextMeshProUGUI speedText;  // Usa TextMeshProUGUI

    private bool wasGrounded;
    private float landSoundCooldown = 1f;  // Duración del cooldown en segundos
    private float landSoundCooldownTimer = 0f; // Contador

    public bool rapido = false;

    public ParticleSystem speedParticles; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles2;

    private Coroutine currentDashEffectCoroutine; // Guarda la corutina actual en ejecución


    public Animator animator;

    private Coroutine currentMoveCoroutine; // Referencia a la corrutina actual
    private Vector3 targetPosition; // Posición objetivo actual
    public LevelManager levelManager;

    void Awake()
    {
        animator.speed = 1;
        speedParticles.Stop();
        speedParticles2.Stop();
        lookSpeed = GameData.sliderValue;

    }
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked; // Para que el cursor no se vea.
        Cursor.visible = false; // Hace invisible el cursor.
        rb.freezeRotation = true;

        originMaxVelocity = maxVelocity;
        originSpeed = moveSpeed;
    }

    void Update()
    {
        if (rb.linearVelocity.y < 0) // Solo cuando el objeto está cayendo
        {
            rb.AddForce(Physics.gravity * (0.01f), ForceMode.Acceleration);
            
        }

        if (landSoundCooldownTimer > 0f)
            {
                landSoundCooldownTimer -= Time.deltaTime;
            }
            if (rb.linearVelocity.y < 0) // Solo cuando cae
            {
                rb.AddForce(Vector3.down * 1.5f, ForceMode.Acceleration); // Aumenta la gravedad
            }


            MovePlayer();

            CheckGrounded();

            PlayFootsteps();


        // Solo saltas si el cooldown terminó
        if (levelManager.playable)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    audioSource.PlayOneShot(jumpSound);
                    Jump();

                }
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
        if (levelManager.playable)
        {
            LookAround();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("ZonaVelocidad"))
        {
            maxVelocity = 60f;
            moveSpeed = 35f;
            stepInterval = 0.2f;

            if (rb.linearVelocity.magnitude > 10)
            {
                speedParticles.Play(); // Activa las partículas
                speedParticles2.Play();
            }
            else if (rb.linearVelocity.magnitude < 10)
            {
                speedParticles.Stop(); // Activa las partículas
                speedParticles2.Stop();
            }



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
            StopAllCoroutines(); // Detenemos cualquier reducción anterior
            StartCoroutine(ReduceSpeedGradually());
        }
    }

    private IEnumerator ReduceSpeedGradually()
    {
        while (maxVelocity > originMaxVelocity || moveSpeed > originSpeed)
        {
            maxVelocity = Mathf.Max(originMaxVelocity, maxVelocity - 5f * Time.deltaTime);
            moveSpeed = Mathf.Max(originSpeed, moveSpeed - 5f * Time.deltaTime);
            yield return null; // Espera un frame antes de continuar
        }

        if (!isMovingTowards)
        {
            speedParticles.Stop();
            speedParticles2.Stop();
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
        // Si ya hay una corrutina corriendo, la detenemos antes de iniciar otra
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }

        // Actualizamos la posición objetivo
        targetPosition = enemyPosition;

        StartCoroutine(MoveAnim());
        audioSource.PlayOneShot(dashSound);

        // Iniciamos la nueva corrutina y guardamos su referencia
        currentMoveCoroutine = StartCoroutine(MoveToPositionCoroutine(enemyPosition));
    }
    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition)
    {
        dashRigidbody.canDash = false;   
            originalVelocity = rb.linearVelocity;

            float journeyLength = Vector3.Distance(transform.position, targetPosition);
            float startTime = Time.time;
            speedParticles.Play(); // Activa las partículas
            speedParticles2.Play();

            // Lerp desde la posición actual hasta la del enemigo
            while (Vector3.Distance(transform.position, targetPosition) > 2f) // Menor tolerancia
            {
                // Si el objetivo cambió, terminamos la corrutina
                if (targetPosition != this.targetPosition)
                {
                    yield break;
                }

                float distanceCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distanceCovered / journeyLength;

                fractionOfJourney = Mathf.Clamp01(fractionOfJourney); // Aseguramos que no se pase del 100%

                transform.position = Vector3.Lerp(transform.position, targetPosition, fractionOfJourney);

                yield return null;
            }

            // Aseguramos que el jugador llegue exactamente a la posición del enemigo
            transform.position = targetPosition;

            speedParticles.Stop(); // Desactiva las partículas
            speedParticles2.Stop(); // Desactiva las partículas
            rb.linearVelocity = originalVelocity;

            // Limpiamos la referencia a la corrutina actual
            currentMoveCoroutine = null;
        dashRigidbody.canDash = true;

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Glovo"))
        {
            fuerzaGlovo = collision.gameObject.GetComponent<Glovo>().fuerza;
            // Si hay una corutina corriendo, la paramos antes de iniciar otra
            if (currentDashEffectCoroutine != null)
            {
                StopCoroutine(currentDashEffectCoroutine);
            }

            currentDashEffectCoroutine = StartCoroutine(DashEffect());
            fall.HUDanimator.ResetTrigger("CantPush"); // Evita que se superpongan triggers
            fall.HUDanimator.SetTrigger("CanPush");
            dashRigidbody.HUDanimator.ResetTrigger("CantDash");
            dashRigidbody.HUDanimator.SetTrigger("CanDash");

            // Lógica del impulso vertical para Glovo
            rb.linearVelocity = new Vector3(rb.linearVelocity.x,fuerzaGlovo, rb.linearVelocity.z);
            fall.canFall = true;
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
            fall.HUDanimator.ResetTrigger("CantPush"); // Evita que se superpongan triggers
            fall.HUDanimator.SetTrigger("CanPush");
            dashRigidbody.HUDanimator.ResetTrigger("CantDash");
            dashRigidbody.HUDanimator.SetTrigger("CanDash");

            // Lógica del impulso vertical para SuperGlovo
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 100f, rb.linearVelocity.z);
            dashRigidbody.canDash = true;
            fall.canFall = true;
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
        speedParticles2.Play();
        yield return new WaitForSeconds(0.5f);
        speedParticles.Stop();
        speedParticles2.Stop();
    }
    public IEnumerator MoveAnim()
    {

        
        yield return new WaitForSeconds(0.1f / 2);
        if (animator != null)
        {
            animator.ResetTrigger("MoveFw");
            animator.SetTrigger("MoveFw");         // Empieza desde el principio sí o sí
        }
        // Espera el tiempo necesario para el dash o la duración de la animación
        yield return new WaitForSeconds(0.3f / 2);
        if (animator != null)
        {
            animator.ResetTrigger("DontMove");
            animator.SetTrigger("DontMove");      // Transición a otro estado si es necesario
        }
        animator.speed = 1f;
        
    }

}

