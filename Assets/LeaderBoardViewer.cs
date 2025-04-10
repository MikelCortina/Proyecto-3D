using LootLocker.Requests;
using UnityEngine;

public class LeaderboardViewer : MonoBehaviour
{
    public string leaderboardKey = "level_1";
    
    public void ObtenerTopScores()
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            if (response.success)
            {
                foreach (var item in response.items)
                {
                    float tiempoEnSegundos = item.score / 100f; // Convertir de int a float con 2 decimales
                    Debug.Log("Jugador: " + item.member_id + " - Tiempo: " + tiempoEnSegundos.ToString("F2") + " s");
                }
            }
            else
            {
                Debug.Log("Error al obtener leaderboard: " + response.errorData.message);
            }
        });
    }
}
