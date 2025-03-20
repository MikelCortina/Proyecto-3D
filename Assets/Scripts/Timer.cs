using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Asigna un UI Text en el Inspector
    public Rigidbody player; // Asigna el Rigidbody del jugador en el Inspector
    private float currentTime;
    private bool isRunning;


    void Awake()
    {
        // Busca el GameObject llamado "Player" en la escena
        GameObject playerObj = GameObject.Find("Player");

        if (playerObj != null)
        {
            player = playerObj.GetComponent<Rigidbody>();

            if (player != null)
            {
                Debug.Log("player Rigidbody asignado automáticamente a: " + playerObj.name);
            }
            else
            {
                Debug.LogWarning("El GameObject 'Player' no tiene un componente Rigidbody");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un GameObject llamado 'Player' en la escena");
        }
    }
    void Start()
    {
    ResetTimer();
    }

    void Update()
    {
        if (!isRunning && player.linearVelocity.magnitude > 0.1f) // Detecta movimiento
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
