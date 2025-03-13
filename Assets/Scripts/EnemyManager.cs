using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Impulsos armaJugador;
    public DashRigidbody dash;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && dash.hasDashed)
        {
            DestroyEnemy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bala"))
        {
            Debug.Log("¡Impacto registrado!");
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnDestroy()
    {
        if (armaJugador != null)
        {
            armaJugador.charger = armaJugador.chargerMax;
        }
        if (dash != null)
        {
            dash.canDash = true;
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
