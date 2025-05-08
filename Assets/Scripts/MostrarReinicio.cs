using UnityEngine;
using TMPro;

public class ShowTextOnCollision : MonoBehaviour
{
    public TextMeshProUGUI hudText;            // Asigna tu texto aquí
    public string targetTag = "Zona 0";    // Asigna el tag que debe tener el objeto con el que colisiona el jugador
    public float showDuration = 3f;           // Cuánto tiempo el texto se mantiene visible
    public float fadeSpeed = 2f;              // Qué tan rápido se desvanece el texto

    private float currentAlpha = 0f;
    private float timer = 0f;
    private bool isColliding = false;

    void Start()
    {
        SetAlpha(0f);  // Inicialmente el texto es invisible
    }

    void Update()
    {
        // Si está colisionando con el objeto y el texto está visible, comienza el temporizador para el desvanecimiento
        if (isColliding)
        {
            timer += Time.deltaTime;

            // Después de un tiempo, empieza a desvanecerse
            if (timer > showDuration)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, 0f, fadeSpeed * Time.deltaTime);
                SetAlpha(currentAlpha);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si la colisión es con un objeto que tiene el tag especificado
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Mostrar el texto al entrar en colisión
            
            SetAlpha(1f); // Hace que el texto sea visible inmediatamente
            isColliding = true; // Marcar que la colisión ha ocurrido
            timer = 0f; // Reiniciar el temporizador para el desvanecimiento
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Si el jugador sale de la colisión, detiene el proceso de desvanecimiento
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Opcionalmente, el texto puede comenzar a desvanecerse al salir de la colisión.
            // Si deseas que no se desvanezca inmediatamente, puedes eliminar este bloque.
            timer = showDuration; // Establecer el tiempo de desvanecimiento al máximo
        }
    }

    // Establecer la opacidad del texto
    void SetAlpha(float alpha)
    {
        var color = hudText.color;
        color.a = alpha;
        hudText.color = color;
    }
}
