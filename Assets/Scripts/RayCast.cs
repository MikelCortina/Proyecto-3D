using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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
    public Animator animator;
    public Image screenEffect;  // Referencia al Image de UI que actuará como efecto

    public ParticleSystem speedParticles; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles2; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles3; // Arrastra el Particle System desde el Inspector
    public ParticleSystem speedParticles4; // Arrastra el Particle System desde el Inspector

    public LevelManager levelManager;
    private void Start()
    {
        speedParticles.Stop();
        speedParticles2.Stop();
        speedParticles3.Stop();
        speedParticles4.Stop();
    }
    void Update()
    {
        if (levelManager.playable == true)
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;  // Actualiza el tiempo del siguiente disparo
                Shoot();
            }
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
                StartCoroutine(PlayScreenEffect());
                
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
                StartCoroutine(PlayScreenEffect());
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
                StartCoroutine(ShootAnim());
                audioSource.PlayOneShot(shootSound);
            }
            else if (audioSource != null && shootSound != null && hit.collider.CompareTag("Enemigo"))
            {
                StartCoroutine(ShootAnim());
                // Si es un enemigo, también se asegura de reproducir el sonido
                audioSource.PlayOneShot(shootSound);
            }
        }
        else
        {
            // Si el raycast no impacta nada, reproducir el sonido
            if (audioSource != null && shootSound != null)
            {
                StartCoroutine(ShootAnim());
                audioSource.PlayOneShot(shootSound);
            }
        }

    }
    IEnumerator ShootAnim()
    {
        speedParticles.Play();
        speedParticles2.Play();
        speedParticles3.Play();
        speedParticles4.Play();
        // Iniciar la animación de disparo
        if (animator != null)
        {
            animator.SetTrigger("Shoot"); // Asegúrate que este trigger existe en tu Animator Controller
        }
        yield return new WaitForSeconds(0.1f);
      
        // Iniciar la animación de disparo
        if (animator != null)
        {
            animator.SetTrigger("DontShoot");  // Asegúrate que este trigger existe en tu Animator Controller
        }
        speedParticles.Stop();
        speedParticles2.Stop();
        speedParticles3.Stop();
        speedParticles4.Stop();

    }
    private IEnumerator PlayScreenEffect()
    {
        // Establece el color blanco con opacidad al inicio (opacidad 0.1f)
        screenEffect.color = new Color(1f, 1f, 1f, 0.025f);  // Blanco con algo de opacidad
        Debug.Log("Inicio - Opacidad: 0.1f");

        // Espera un momento para que el efecto sea visible
        yield return new WaitForSeconds(0.05f);

        // Desvanecimiento (fade out)
        float timeElapsed = 0f;
        float fadeDuration = 0.3f;  // Duración del desvanecimiento

        while (timeElapsed < fadeDuration)
        {
            // Lerp para hacer un fade de opacidad de 0.1 a 0
            float alphaValue = Mathf.Lerp(0.025f, 0f, timeElapsed / fadeDuration);
            screenEffect.color = new Color(1f, 1f, 1f, alphaValue);
            Debug.Log("Alpha: " + alphaValue);  // Imprime el valor de alpha para verificar que cambia
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que el valor alpha sea 0 al final del fade
        screenEffect.color = new Color(1f, 1f, 1f, 0f);  // Establece completamente transparente
        Debug.Log("Final - Opacidad: 0f");
    }


}