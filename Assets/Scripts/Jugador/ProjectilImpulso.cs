using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    public float speed;  // Velocidad del proyectil
    public float lifetime; // Tiempo hasta que el proyectil desaparezca

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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
        if (other.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("Enemigo");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
