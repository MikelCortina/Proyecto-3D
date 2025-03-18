using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float lifetime = 5f;  // Tiempo de vida del proyectil

    private float speed;
    private Vector3 direction;
    private float oscillationAmplitude;
    private float oscillationFrequency;
    private bool oscillating = false;
    private float startTime;
   
  

    private void Start()
    {
        // Destruir el proyectil después de un tiempo
        Destroy(gameObject, lifetime);
        startTime = Time.time;
      
    }

    public void MoveProjectile(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    public void StartOscillation(float amplitude, float frequency)
    {
        oscillationAmplitude = amplitude;
        oscillationFrequency = frequency;
        oscillating = true;
    }

    private void Update()
    {
        // Movimiento del proyectil
        Vector3 movement = direction * speed * Time.deltaTime;

        if (oscillating)
        {
            float oscillationOffset = Mathf.Sin((Time.time - startTime) * oscillationFrequency) * oscillationAmplitude;
            movement.y += oscillationOffset;
        }

        transform.Translate(movement, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
       
         if (other.gameObject.CompareTag("Enemigo"))
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
