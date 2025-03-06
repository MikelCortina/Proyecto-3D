using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;  // Velocidad del proyectil
    public float lifetime = 3f; // Tiempo hasta que el proyectil desaparezca

    void Start()
    {
        // Destruir el proyectil después de un tiempo
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Aquí puedes añadir lógica para cuando el proyectil golpea algo (por ejemplo, destruir el proyectil o hacer daño)
        Destroy(gameObject); // Destruir el proyectil cuando golpea algo
    }
}
