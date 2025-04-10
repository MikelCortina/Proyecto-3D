using UnityEngine;
using LootLocker.Requests;

public class LeaderboardUploader : MonoBehaviour
{
    public string leaderboardKey;
    string playerName;
    public void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "guest_player");
    }
    public void EnviarPuntaje(int score)
    {
        LootLockerSDKManager.SubmitScore(playerName, score, leaderboardKey, (response) =>
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

