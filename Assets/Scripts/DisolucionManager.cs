using UnityEngine;
using System.Collections;

public class DissolveController : MonoBehaviour
{
    public Material dissolveMaterial;  // Asigna el material con el shader de disolución
    private float dissolveAmount = 0f;
    public float dissolveSpeed = 1f;

    private bool isDissolving = false;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(dissolveMaterial); // Asegurar que cada enemigo tiene su propio material
        }
    }

    public void StartDissolve()
    {
        if (!isDissolving)
        {
            isDissolving = true;
            StartCoroutine(DissolveEffect());
        }
    }

    IEnumerator DissolveEffect()
    {
        while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            rend.material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }

        Destroy(gameObject); // Destruir el enemigo al finalizar la disolución
    }
}
