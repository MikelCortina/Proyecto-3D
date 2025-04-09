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
                    Debug.Log("Jugador: " + item.member_id + " - Puntos: " + item.score);
                }
            }
            else
            {
                Debug.Log("Error al obtener leaderboard: " + response.errorData.message);
            }
        });
    }
}
