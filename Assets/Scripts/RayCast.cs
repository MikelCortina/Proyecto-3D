using UnityEngine;
using UnityEngine.Audio;

public class RayCast : MonoBehaviour
{
    public float distanciDisparo = 500f;
    public float fuerzaDisparo = 15f;
    public float fireRate = 0.3f;  // Tiempo entre disparos
    private float nextFireTime = 0f;  // Control de tiempo de disparo
    public Camera cam;
    public LayerMask layerIgnorar; // Asigna esto en el Inspector para excluir capas
    public PlayerMovement jugador;
    public Impulsos armaJugador;
    public DashRigidbody dash;
    public bool soundEffectYoN;
    public AudioClip shootSound; // Clip de sonido del disparo
    public AudioSource audioSource; // Fuente de audio

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
                hit.collider.gameObject.GetComponent<EnemyHealth>().IniciarDisolucion();

                if (armaJugador != null)
                {
                    armaJugador.charger = armaJugador.chargerMax;
                }

                if (dash != null)
                {
                    dash.canDash = true;
                }
            }
            else if (hit.collider.CompareTag("EnemigoMovimiento"))
            {
                jugador.MoveToEnemy(hit.collider.gameObject.GetComponent<Transform>().position);
                hit.collider.gameObject.GetComponent<EnemyHealth>().IniciarDisolucion();

                if (armaJugador != null)
                {
                    armaJugador.charger = armaJugador.chargerMax;
                }

                if (dash != null)
                {
                    dash.canDash = true;
                }

                soundEffectYoN = false; // No reproducir sonido cuando el tag es "EnemigoMovimiento"
            }

            // Si el objeto golpeado tiene un Rigidbody, aplica una fuerza
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(ray.direction * fuerzaDisparo, hit.point, ForceMode.Impulse);
            }

            // Reproducir sonido en todos los casos excepto cuando el tag es "EnemigoMovimiento"
            if (audioSource != null && shootSound != null && !hit.collider.CompareTag("EnemigoMovimiento"))
            {
                audioSource.PlayOneShot(shootSound);
            }
            else if (audioSource != null && shootSound != null && hit.collider.CompareTag("Enemigo"))
            {
                // Si es un enemigo, también se asegura de reproducir el sonido
                audioSource.PlayOneShot(shootSound);
            }
        }
        else
        {
            // Si el raycast no impacta nada, reproducir el sonido
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }

    }



}