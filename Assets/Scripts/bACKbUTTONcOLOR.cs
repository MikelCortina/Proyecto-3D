using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CambiarColorPadreEHijos : MonoBehaviour
{
    // Colores para el padre
    public string hexPadreNormal = "#FFD700";      
    public string hexPadreResaltado = "#000000";   

    // Colores para los hijos
    public string hexHijosNormal = "#000000";      
    public string hexHijosResaltado = "#FFFFFF";   

    private Color colorPadreNormal;
    private Color colorPadreResaltado;

    private Color colorHijosNormal;
    private Color colorHijosResaltado;

    void Start()
    {
        // Convertir los HEX en colores
        ColorUtility.TryParseHtmlString(hexPadreNormal, out colorPadreNormal);
        ColorUtility.TryParseHtmlString(hexPadreResaltado, out colorPadreResaltado);

        ColorUtility.TryParseHtmlString(hexHijosNormal, out colorHijosNormal);
        ColorUtility.TryParseHtmlString(hexHijosResaltado, out colorHijosResaltado);

        // Poner los colores normales al iniciar
        CambiarColores(colorPadreNormal, colorHijosNormal);
    }

    public void CambiarAColoresResaltados()
    {
        CambiarColores(colorPadreResaltado, colorHijosResaltado);
    }

    public void CambiarAColoresNormales()
    {
        CambiarColores(colorPadreNormal, colorHijosNormal);
    }

    private void CambiarColores(Color colorPadre, Color colorHijos)
    {
        // Cambia el color del padre
        CambiarColorEnEste(gameObject, colorPadre);

        // Cambia el color de los hijos (recursivamente)
        foreach (Transform hijo in transform)
        {
            CambiarColorRecursivo(hijo, colorHijos);
        }
    }

    private void CambiarColorRecursivo(Transform trans, Color color)
    {
        CambiarColorEnEste(trans.gameObject, color);

        foreach (Transform hijo in trans)
        {
            CambiarColorRecursivo(hijo, color);
        }
    }

    private void CambiarColorEnEste(GameObject obj, Color color)
    {
        // Cambia color de imagen si tiene
        Image img = obj.GetComponent<Image>();
        if (img != null)
        {
            img.color = color;
            Debug.Log($"Cambiando color de Image en {obj.name} a {color}");
        }
        else
        {
            Debug.LogWarning($"El objeto {obj.name} NO tiene un componente Image para cambiar el color.");
        }

        // Cambia color de texto (UI clásico)
        Text txt = obj.GetComponent<Text>();
        if (txt != null)
        {
            txt.color = color;
            Debug.Log($"Cambiando color de Text en {obj.name} a {color}");
        }

        // Cambia color de TextMeshProUGUI (opcional)
        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.color = color;
            Debug.Log($"Cambiando color de TextMeshPro en {obj.name} a {color}");
        }
    }

}
