using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LootLocker.Requests;


public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Asigna un UI Text en el Inspector
    public Rigidbody player; // Asigna el Rigidbody del jugador en el Inspector
    private float currentTime;
    private bool isRunning;

    public LevelManager levelManager;

    void Awake()
    {

        LootLockerSDKManager.StartGuestSession((response) => {
            if (response.success)
            {
                Debug.Log("Sesión iniciada");
            }
            else
            {
                Debug.LogError("Error al iniciar sesión");
            }
        });

    }
    void Start()
    {
    ResetTimer();
    }

    void Update()
    {
        if (levelManager.playable == true)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
            {
                StartTimer();
            }
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
