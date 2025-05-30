using UnityEngine;
using UnityEngine.UI;

public class ToggleControlador : MonoBehaviour
{
    public Toggle miToggle;                  // Asigna el Toggle desde el Inspector
    public GameObject objetoAControlar;      // Asigna el objeto que quieres activar/desactivar

    void Start()
    {
        // Asegura que el estado inicial esté sincronizado
        objetoAControlar.SetActive(miToggle.isOn);

        // Escucha los cambios del toggle
        miToggle.onValueChanged.AddListener((bool valor) =>
        {
            objetoAControlar.SetActive(valor);
        });
    }
}
