using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public LayerMask enemyLayer; // Para detectar enemigos con Raycast

    void Start()
    {
        Destroy(gameObject, lifetime); // Destruir después de un tiempo
    }

    void Update()
    {
        // Guardar la posición anterior para hacer el Raycast
        Vector3 previousPosition = transform.position;

        // Mover la bala hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Comprobar si la bala impactó con algo usando un Raycast
        RaycastHit hit;
        if (Physics.Linecast(previousPosition, transform.position, out hit, enemyLayer))
        {
            if (hit.collider.CompareTag("Enemigo"))
            {
                Debug.Log("Impacto forzado con Raycast en " + hit.collider.name);
                Destroy(hit.collider.gameObject); // Destruir enemigo
                Destroy(gameObject); // Destruir bala
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("¡Impacto directo en enemigo!");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
