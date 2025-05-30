using UnityEngine;
using LootLocker.Requests;

public class LootLockerInit : MonoBehaviour
{
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Sesión de invitado iniciada correctamente");
            }
            else
            {
                Debug.LogError("Error al iniciar sesión: " + response.errorData.message);
            }
        });
    }
}
