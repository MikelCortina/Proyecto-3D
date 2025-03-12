using UnityEngine;

public class RayCast : MonoBehaviour
{
    public float distanciDisparo = 500f;
    public float fuerzaDisparo = 15f;
    public float fireRate = 0.5f;  // Tiempo entre disparos
    private float nextFireTime = 0f;  // Control de tiempo de disparo
    public Camera cam;
    public LayerMask layerIgnorar; // Asigna esto en el Inspector para excluir capas

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;  // Actualiza el tiempo del siguiente disparo
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Raycast desde el centro de la pantalla
        RaycastHit hit;

        // Excluir la capa de las balas usando una máscara de capas
        if (Physics.Raycast(ray, out hit, distanciDisparo, ~layerIgnorar))  // "~" invierte la máscara para excluir la capa
        {
            Debug.DrawRay(ray.origin, ray.direction * distanciDisparo, Color.red, 1f); // Visualizar el raycast

            // Si golpea un enemigo, lo destruye
            if (hit.collider.CompareTag("Enemigo"))
            {
                Destroy(hit.collider.gameObject);
            }

            // Si el objeto golpeado tiene un Rigidbody, aplica una fuerza
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(ray.direction * fuerzaDisparo, hit.point, ForceMode.Impulse);
            }
        }
    }
}
