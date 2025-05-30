using UnityEngine;

public class ObjetoPersistente : MonoBehaviour
{
    private static ObjetoPersistente instanciaUnica;

    void Awake()
    {
        if (instanciaUnica == null)
        {
            instanciaUnica = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instanciaUnica != this)
        {
            Destroy(gameObject);
        }
    }
}
