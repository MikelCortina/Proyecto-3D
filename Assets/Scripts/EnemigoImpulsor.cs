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
                dash.isDashing = false;
                // Reinicia la velocidad del jugador antes de aplicar la fuerza
                playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
                playerRb.angularVelocity = Vector3.zero;

                // Aplica la nueva fuerza
                playerRb.linearVelocity += Vector3.up * 10f;
            }
            dash.canDash = true;

            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        
    }

}
