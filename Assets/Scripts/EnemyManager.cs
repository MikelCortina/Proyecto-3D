using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Impulsos armaJugador;
    public DashRigidbody dash;
    public Renderer enemyRenderer; // Asegúrate de asignar el Renderer en el Inspector
    private MaterialPropertyBlock propertyBlock;
    public float tiempoDisolucion = 1.0f; // Tiempo total de la animación

    private void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && dash.hasDashed)
        {
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
        if (armaJugador != null)
        {
            armaJugador.charger = armaJugador.chargerMax;
        }
        if (dash != null)
        {
            dash.canDash = true;
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}