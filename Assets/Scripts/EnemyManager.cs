using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Impulsos armaJugador;
    private DashRigidbody dash;
    private Renderer enemyRenderer;
    private MaterialPropertyBlock propertyBlock;
    private float tiempoDisolucion = 0.25f;
    private Transform jugador;
    private LevelManager levelExit;
    public AudioClip destroySound;
    public AudioSource audioSource;
    public float explosionForce ;
    public float explosionRadius;
    public float shrinkDuration = 1.5f;

    private void Start()
    {
        armaJugador = FindObjectOfType<Impulsos>();
        dash = FindObjectOfType<DashRigidbody>();
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null) jugador = jugadorObj.transform;
        propertyBlock = new MaterialPropertyBlock();
        enemyRenderer = GetComponent<Renderer>() ?? GetComponentInChildren<Renderer>();
        GameObject salidaObj = GameObject.FindGameObjectWithTag("Salida");
        if (salidaObj != null) levelExit = salidaObj.GetComponent<LevelManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && dash != null && dash.hasDashed)
        {
            if (armaJugador != null) armaJugador.charger = armaJugador.chargerMax;
            if (dash != null) dash.canDash = true;
            if (PuedeVerAlJugador()) IniciarDisolucion();
        }
    }

    private bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;
        Vector3 direccion = (jugador.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direccion, out RaycastHit hit))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    public void IniciarDisolucion()
    {
        audioSource.PlayOneShot(destroySound);
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
        ExplodeChildren();
        DestroyEnemy();
    }

    private void AplicarDisolucion(float valorDisolucion)
    {
        if (enemyRenderer == null) return;
        enemyRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_Disolucion_Inicial", valorDisolucion);
        enemyRenderer.SetPropertyBlock(propertyBlock);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void ExplodeChildren()
    {
        foreach (Transform child in transform)
        {
            child.SetParent(null);
            if (child.TryGetComponent(out Rigidbody rb))
            {
                Vector3 explosionDir = (child.position - transform.position).normalized;
                rb.isKinematic = false;
                rb.AddForce(explosionDir * explosionForce, ForceMode.Impulse);
            }
            StartCoroutine(ShrinkAndDestroy(child));
        }
    }

    IEnumerator ShrinkAndDestroy(Transform obj)
    {
        float elapsed = 0f;
        Vector3 initialScale = obj.localScale;
        while (elapsed < shrinkDuration)
        {
            float scaleFactor = Mathf.Lerp(1f, 0f, elapsed / shrinkDuration);
            obj.localScale = initialScale * scaleFactor;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(obj.gameObject);
    }

    private void OnDestroy()
    {
        if (levelExit != null) levelExit.killCount++;
    }
}
