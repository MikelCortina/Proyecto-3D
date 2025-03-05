using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float sprintSpeed = 20f;
    public float lookSpeed = 2f;
    public float maxLookAngle = 80f;
    public float jumpForce = 10f;
    public float gravityMultiplier = 2f; // Para ajustar la gravedad en el salto
    public float inertiaFactor = 5f; // Factor de inercia

    private Rigidbody rb;
    public Camera playerCamera;
    private float rotationX = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Para que el cursor no se vea.
        Cursor.visible = false; // Hace invisible el cursor.

        // Asegúrate de que el Rigidbody no rote por sí mismo.
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Controlar el movimiento del jugador
        MovePlayer();
        // Controlar la rotación de la cámara
        LookAround();
        // Comprobar si está en el suelo
        CheckGrounded();
        // Controlar el salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Si se presiona el shift, se mueve más rápido
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Aplicar inercia en el movimiento
        Vector3 targetVelocity = moveDirection * speed;
        Vector3 velocity = Vector3.Lerp(rb.linearVelocity, new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z), Time.deltaTime * inertiaFactor);
        rb.linearVelocity = velocity;
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);

        // Rotación de cámara
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        // Rotación del cuerpo (solo en el eje Y)
        transform.Rotate(Vector3.up * mouseX);
    }

    void CheckGrounded()
    {
        // Comprobar si el jugador está en el suelo utilizando un Raycast
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f); // Ajusta el valor 1.1f según el tamaño del jugador
    }

    void Jump()
    {
        // Solo saltar si estamos en el suelo
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
