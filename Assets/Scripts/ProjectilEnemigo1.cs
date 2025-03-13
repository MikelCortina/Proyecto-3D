using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float lifetime = 5f;  // Tiempo de vida del proyectil

    private float speed;
    private Vector3 direction;

    private void Start()
    {
        // Destruir el proyectil después de un tiempo
        Destroy(gameObject, lifetime);
    }

    public void MoveProjectile(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    private void Update()
    {
        // Mueve el proyectil manualmente en la dirección
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Te han dado");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemigo"))
        {

        }
        else if (other.gameObject.CompareTag("BalaEnemy"))
        {

        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
