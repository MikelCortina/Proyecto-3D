using LootLocker.Requests;
using UnityEngine;

public class BestTimesManager : MonoBehaviour
{
    private const int maxTimes = 3; // Número máximo de mejores tiempos guardados
    private float[] bestTimes = new float[maxTimes];
    public LeaderboardUploader leaderboardUploader; // Referencia al script de LootLocker
    public LeaderboardViewer leaderboardViewer;

    void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1); // Obtener el nivel actual
        LoadBestTimes(currentLevel);
    }

    public void SaveNewTime(float newTime, int level)
    {
        LoadBestTimes(level); // Cargar los tiempos actuales del nivel

        for (int i = 0; i < bestTimes.Length; i++)
        {
            if (newTime < bestTimes[i] || bestTimes[i] == 0)
            {
                for (int j = bestTimes.Length - 1; j > i; j--)
                {
                    bestTimes[j] = bestTimes[j - 1];
                }
                bestTimes[i] = newTime;
                break;
            }
        }
        SaveBestTimes(level); // Guardar los mejores tiempos del nivel

        // Convertir el tiempo a centésimas de segundo (para enviar como int)
        int tiempoFormateado = Mathf.RoundToInt(newTime * 100);

        // Asegurar que se use la clave del leaderboard correspondiente al nivel
        leaderboardUploader.leaderboardKey = "level" + level;
        leaderboardViewer.leaderboardKey = "level" + level;

        // Enviar puntaje a LootLocker
        leaderboardUploader.EnviarPuntaje(tiempoFormateado);
        leaderboardViewer.ObtenerTopScores();
    }

    public float[] GetBestTimes(int level)
    {
        LoadBestTimes(level);
        return bestTimes;
    }

    private void SaveBestTimes(int level)
    {
        for (int i = 0; i < bestTimes.Length; i++)
        {
            PlayerPrefs.SetFloat($"BestTime_Level{level}_{i}", bestTimes[i]);
        }
        PlayerPrefs.Save();
    }

    private void LoadBestTimes(int level)
    {
        for (int i = 0; i < bestTimes.Length; i++)
        {
            bestTimes[i] = PlayerPrefs.GetFloat($"BestTime_Level{level}_{i}", 0f);
        }
    }
}
