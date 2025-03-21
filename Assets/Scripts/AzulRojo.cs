using UnityEngine;
using UnityEngine.UI;

public class CambiarColorHex2 : MonoBehaviour
{
    private Image imagenBoton;

    private Color colorNormal;
    private Color colorResaltado;

    void Start()
    {
        imagenBoton = GetComponent<Image>();

        // Asignar colores usando código HEX
        ColorUtility.TryParseHtmlString("#69CDDD", out colorNormal);     // Azul Claro (SkyBlue)
        ColorUtility.TryParseHtmlString("#FFFFFF", out colorResaltado);  // Dorado (Gold)
        
        // Establecer el color inicial
        imagenBoton.color = colorNormal;
    }

    public void CambiarAColorResaltado()
    {
        imagenBoton.color = colorResaltado;
    }

    public void CambiarAColorNormal()
    {
        imagenBoton.color = colorNormal;
    }
}
