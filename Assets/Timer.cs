using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Asigna un UI Text en el Inspector
    public Rigidbody playerRigidbody; // Asigna el Rigidbody del jugador en el Inspector
    private float currentTime;
    private bool isRunning;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!isRunning && playerRigidbody.linearVelocity.magnitude > 0.1f) // Detecta movimiento
        {
            StartTimer();
        }

        if (isRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        isRunning = false;
        UpdateTimerDisplay();
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = currentTime.ToString("F2"); // Muestra con 2 decimales
    }
}
