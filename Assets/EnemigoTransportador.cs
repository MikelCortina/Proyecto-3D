using UnityEngine;

public class EnemigoTransportador : MonoBehaviour
{
    public PlayerMovement jugador;

    private void OnDestroy()
    {
        jugador.MoveToEnemy(transform.position);
    }

}
        
    


