using UnityEngine;

public class TrapMovement : MonoBehaviour
{
    public Transform trap; // La trampa que se moverá
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float stopDistance = 0.5f; // Distancia a la que la trampa deja de moverse

    public bool lockX = false; // Bloquear movimiento en el eje X
    public bool lockY = true;  // Bloquear movimiento en el eje Y (activado por defecto)
    public bool lockZ = false; // Bloquear movimiento en el eje Z

    private Transform target; // Referencia al jugador

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Detecta al jugador
        {
            target = other.transform; // Guarda la referencia del jugador
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Detecta al jugador
        {
            target = null; // Borra la referencia del jugador
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            Vector3 currentPosition = trap.position;

            // Bloquear los ejes según las variables públicas
            if (lockX) targetPosition.x = currentPosition.x;
            if (lockY) targetPosition.y = currentPosition.y;
            if (lockZ) targetPosition.z = currentPosition.z;

            // Mueve la trampa hacia la posición del jugador respetando los ejes bloqueados
            trap.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

            // Si la trampa llega a la distancia mínima, deja de moverse
            if (Vector3.Distance(currentPosition, targetPosition) < stopDistance)
            {
                target = null;
            }
        }
    }
}
