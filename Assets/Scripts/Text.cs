using UnityEngine;
using TMPro;

public class FadeTextOnMove : MonoBehaviour
{
    public Transform player;                    // Asigna tu jugador aquí
    public TextMeshProUGUI hudText;             // Asigna tu texto aquí
    public float fadeSpeed = 3.5f;                // Qué tan rápido se desvanece
    public float showDuration = 1f;             // Cuánto dura visible antes de desvanecerse

    private Vector3 lastPosition;
    private float currentAlpha = 1f;
    private float timer = 0f;
    private bool isMoving = false;

    void Start()
    {
        lastPosition = player.position;
        SetAlpha(1f);
    }

    void Update()
    {
        // Detectar si el jugador se ha movido
        if (Vector3.Distance(player.position, lastPosition) > 0.05f) // Detecta el movimiento
        {
            if (!isMoving)
            {
                isMoving = true; // Empieza el movimiento
                timer = 0f; // Reinicia el temporizador
            }
        }
        else
        {
            // Si el jugador no se ha movido y ha pasado el tiempo, comienza a desvanecerse
            if (isMoving)
            {
                timer += Time.deltaTime;
            }
        }

        // Lógica para desvanecer
        if (timer > showDuration)
        {
            currentAlpha = Mathf.Lerp(currentAlpha, 0f, fadeSpeed * Time.deltaTime);
        }

        SetAlpha(currentAlpha);
        lastPosition = player.position;
    }

    void SetAlpha(float alpha)
    {
        var color = hudText.color;
        color.a = alpha;
        hudText.color = color;
    }
}
