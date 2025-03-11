using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections; // Importa el espacio de nombres para TextMesh Pro

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float sprintSpeed = 20f;
    public float lookSpeed = 2f;
    public float maxLookAngle = 80f;
    public float jumpForce = 10f;
    public float inertiaFactor = 5f; // Factor de inercia
    public float maxVelocity; // Velocidad máxima permitida
    private float originSpeed;
    private float originMaxVelocity;
    private Vector3 originalVelocity;
    private float saltosTot = 1; //Contador del doble salto
   

    private Rigidbody rb;
    public Camera playerCamera;
    private float rotationX = 0f;
    private bool isGrounded;

    // Referencia al componente TextMesh Pro para mostrar la velocidad
    public TextMeshProUGUI speedText;  // Usa TextMeshProUGUI

    // NUEVO:
    public float jumpCooldown = 0.2f; // Cooldown entre saltos
    private float jumpCooldownTimer = 0f; // Temporizador para controlar el cooldown

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
        MovePlayer();

        CheckGrounded();

        // Restar tiempo al cooldown
        if (jumpCooldownTimer > 0)
        {
            jumpCooldownTimer -= Time.deltaTime;
        }

        // Solo saltas si el cooldown terminó
        if (Input.GetButtonDown("Jump") && jumpCooldownTimer <= 0)
        {
            if (isGrounded)
            {
                Jump();
                saltosTot++;
                jumpCooldownTimer = jumpCooldown; // Reinicia el cooldown
            }
            else if (saltosTot < 1)
            {
                Jump();
                saltosTot++;
                jumpCooldownTimer = jumpCooldown; // Reinicia el cooldown
            }
        }

        // Si estás en el suelo, reseteas el contador de saltos
        if (isGrounded)
        {
            saltosTot = 0;
        }

        DisplaySpeed();
        LookAround();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("ZonaVelocidad"))
        {
            maxVelocity = 50f;
            moveSpeed = 30f;
        }
        else
        {
            maxVelocity = originMaxVelocity;
            moveSpeed = originSpeed;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ZonaVelocidad"))
        {
            maxVelocity = originMaxVelocity;
            moveSpeed = originSpeed;
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
       
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f); // Ajusta el valor 1.1f según el tamaño del jugador
        
    }

    void Jump()
    {
        if (isGrounded | saltosTot<2)
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
        StartCoroutine(MoveToPositionCoroutine(enemyPosition));
    }
    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition)
    {
       
        originalVelocity = rb.linearVelocity;

        float journeyLength = Vector3.Distance(transform.position, targetPosition);
        float startTime = Time.time;

        // Lerp desde la posición actual hasta la del enemigo
        while (Vector3.Distance(transform.position, targetPosition) > 4f) // Menor tolerancia
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            fractionOfJourney = Mathf.Clamp01(fractionOfJourney); // Aseguramos que no se pase del 100%

            transform.position = Vector3.Lerp(transform.position, targetPosition, fractionOfJourney);

            yield return null;
        }

        // Aseguramos que el jugador llegue exactamente a la posición del enemigo
        transform.position = targetPosition;

        // Restauramos la velocidad original
        rb.linearVelocity = originalVelocity;
       
    }



}
