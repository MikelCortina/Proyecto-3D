using UnityEngine;
using UnityEngine.UI;

public class CambiarColorHex : MonoBehaviour
{
    private Image imagenBoton;

    private Color colorNormal;
    private Color colorResaltado;

    void Start()
    {
        imagenBoton = GetComponent<Image>();

        // Asignar colores usando código HEX
        ColorUtility.TryParseHtmlString("#FFD700", out colorNormal);     // Azul Claro (SkyBlue)
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
