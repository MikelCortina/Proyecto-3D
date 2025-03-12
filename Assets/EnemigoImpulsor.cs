using UnityEngine;

public class EnemigoImpulsor : MonoBehaviour
{
    public Impulsos armaJugador;
    public DashRigidbody dash;
    public GameObject projectilePrefab;  // Prefab del proyectil
    public Transform player;  // Referencia al jugador

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")&&dash.isDashing)
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Reinicia la velocidad del jugador antes de aplicar la fuerza
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero; // Tambi�n resetea la rotaci�n si es necesario

                // Aplica la nueva fuerza
                playerRb.AddForce(Vector3.up * 1000f, ForceMode.Impulse);

                dash.isDashing = false;
            }
            dash.canDash = true;

            Destroy(gameObject);
        }
    }

}
