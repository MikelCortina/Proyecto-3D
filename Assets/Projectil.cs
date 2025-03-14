using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;  // Velocidad del proyectil
    public float lifetime = 3f; // Tiempo hasta que el proyectil desaparezca

    void Start()
    {
        // Destruir el proyectil despu�s de un tiempo
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Aqu� puedes a�adir l�gica para cuando el proyectil golpea algo (por ejemplo, destruir el proyectil o hacer da�o)
        Destroy(gameObject); // Destruir el proyectil cuando golpea algo
    }
}
