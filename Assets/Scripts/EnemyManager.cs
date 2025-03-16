using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Impulsos armaJugador;
    public DashRigidbody dash;
    private Renderer enemyRenderer; // Se asignará automáticamente si no se asigna manualmente en el Inspector
    private MaterialPropertyBlock propertyBlock;
    private float tiempoDisolucion = 0.25f; // Tiempo total de la animación

    private void Start()
    {
        // Inicializa el bloque de propiedades para cambiar los valores del shader
        propertyBlock = new MaterialPropertyBlock();

        // Asigna el renderer automáticamente si no lo has hecho
        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponent<Renderer>();
            if (enemyRenderer == null)
            {
                enemyRenderer = GetComponentInChildren<Renderer>();
            }
        }

        if (enemyRenderer != null)
        {
            // Cambia el shader del material principal
            Shader shaderNuevo = Shader.Find("Nombre/Del/Shader");
            if (shaderNuevo != null)
            {
                enemyRenderer.material.shader = shaderNuevo;
            }
            else
            {
                Debug.LogWarning("No se encontró el shader especificado.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && dash.hasDashed)
        {
            if (armaJugador != null)
            {
                armaJugador.charger = armaJugador.chargerMax;
            }

            if (dash != null)
            {
                dash.canDash = true;
            }
            IniciarDisolucion();
        }
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
            float valorDisolucion = Mathf.Lerp(inicio, fin, tiempo / duracion); // Interpolación
            AplicarDisolucion(valorDisolucion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        AplicarDisolucion(fin); // Asegurar que termine en el valor exacto
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
            Destroy(gameObject, 0.1f);

        }
    }

    private void OnDestroy()
    {
       
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
