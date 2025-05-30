using UnityEngine;

public class VolumePersistente : MonoBehaviour
{
    private static VolumePersistente instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instancia != this)
        {
            Destroy(gameObject);
        }
    }
}
