using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class PlayerLogin : MonoBehaviour
{
    public TMP_InputField nombreInput;
    public GameObject nombrePanel; // UI para pedir el nombre

    void Start()
    {
        // Si ya hay un nombre guardado, iniciar sesión directamente
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string nombreGuardado = PlayerPrefs.GetString("PlayerName");
            nombrePanel.SetActive(false); // ocultar el panel de nombre
            IniciarSesion(nombreGuardado);
        }
        else
        {
            // Mostrar el panel para que el jugador escriba su nombre
            nombrePanel.SetActive(true);
        }
    }

    // Llamado al presionar el botón "Guardar Nombre"
    public void GuardarNombre()
    {
        string nombre = nombreInput.text;

        if (!string.IsNullOrEmpty(nombre))
        {
            PlayerPrefs.SetString("PlayerName", nombre);
            nombrePanel.SetActive(false);
            IniciarSesion(nombre);
        }
        else
        {
            Debug.LogWarning("El nombre está vacío.");
        }
    }

    void IniciarSesion(string playerName)
    {
        LootLockerSDKManager.StartGuestSession(playerName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Sesión iniciada con nombre: " + playerName);
            }
            else
            {
                Debug.LogError("Error al iniciar sesión: " + response.errorData.message);
            }
        });
    }
}
