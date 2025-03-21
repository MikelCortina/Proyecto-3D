using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Impulsos armaJugador;
    private DashRigidbody dash;
    private Renderer enemyRenderer;
    private MaterialPropertyBlock propertyBlock;
    private float tiempoDisolucion = 0.25f;
    private Transform jugador; // Referencia al jugador
    private LevelExit levelExit;

    public AudioClip destroySound;
    public AudioSource audioSource;

    private void Start()
    {
        // Buscar el arma del jugador en la escena
        armaJugador = FindObjectOfType<Impulsos>();

        // Buscar el dash del jugador en la escena
        dash = FindObjectOfType<DashRigidbody>();

        // Buscar al jugador en la escena
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player'.");
        }

        // Inicializa el bloque de propiedades para cambiar los valores del shader
        propertyBlock = new MaterialPropertyBlock();

        // Asigna el renderer automáticamente
        enemyRenderer = GetComponent<Renderer>() ?? GetComponentInChildren<Renderer>();

       
        GameObject salidaObj = GameObject.FindGameObjectWithTag("Salida");
        if (salidaObj != null)
        {
            levelExit = salidaObj.GetComponent<LevelExit>();
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con el tag 'Salida'.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && dash != null && dash.hasDashed)
        {
            if (armaJugador != null)
            {
                armaJugador.charger = armaJugador.chargerMax;
            }

            if (dash != null)
            {
                dash.canDash = true;
            }

            if (PuedeVerAlJugador()) // Verifica si el enemigo tiene línea de visión con el jugador
            {

                IniciarDisolucion();
            }
        }
    }

    private bool PuedeVerAlJugador()
    {
        if (jugador == null) return false; // Si no hay referencia al jugador, no puede verlo

        Vector3 direccion = (jugador.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direccion, out hit))
        {
            // Si el objeto impactado no es el jugador, entonces hay un obstáculo en el camino
            if (!hit.collider.CompareTag("Player"))
            {
                Debug.Log("El enemigo no puede ver al jugador. Hay un obstáculo: " + hit.collider.name);
                return false;
            }
        }

        return true;
    }

    public void IniciarDisolucion()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DisolverCoroutine(-0.80f, 0.60f, tiempoDisolucion));
    }

    IEnumerator DisolverCoroutine(float inicio, float fin, float duracion)
    {
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float valorDisolucion = Mathf.Lerp(inicio, fin, tiempo / duracion);
            AplicarDisolucion(valorDisolucion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        AplicarDisolucion(fin);
        DestroyEnemy();
    }

    private void AplicarDisolucion(float valorDisolucion)
    {
        if (enemyRenderer == null)
        {
            Debug.LogWarning($"No se puede aplicar la disolución porque enemyRenderer no está asignado en {gameObject.name}.");
            return;
        }

        enemyRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_Disolucion_Inicial", valorDisolucion);
        enemyRenderer.SetPropertyBlock(propertyBlock);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bala"))
        {
            Debug.Log("¡Impacto registrado!");

            if (PuedeVerAlJugador()) // Solo se destruye si tiene línea de visión con el jugador
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        levelExit.killCount++;
    }
}
