using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI displayText; // Referencia al texto

    void Start()
    {
        // Obtener el botón y asignar el evento
        GetComponent<Button>().onClick.AddListener(WriteHello);
    }

    void WriteHello()
    {
        Debug.Log( "Hola");
    }
}
