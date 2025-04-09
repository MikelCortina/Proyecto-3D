using UnityEngine;
using LootLocker.Requests;

public class LeaderboardUploader : MonoBehaviour
{
    public string leaderboardKey;

    public void EnviarPuntaje(int score)
    {
        LootLockerSDKManager.SubmitScore(leaderboardKey, score, "guest_player", (response) =>
        {
            if (response.success)
            {
                Debug.Log("Puntaje enviado correctamente: " + score);
            }
            else
            {
                Debug.LogError("Error al enviar puntaje: " + response.errorData.message);
            }
        });
    }
}

