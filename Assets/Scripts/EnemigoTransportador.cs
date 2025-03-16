using UnityEngine;

public class EnemigoTransportador : MonoBehaviour
{
    public PlayerMovement jugador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bala"))
        {

            jugador.MoveToEnemy(transform.position);
        }
    }

}
        
    


