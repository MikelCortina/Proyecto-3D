using UnityEngine;

public class BestTimesManager : MonoBehaviour
{
    private float[] bestTimes = new float[3]; // Almacena los 3 mejores tiempos

    void Start()
    {
        LoadBestTimes();
    }

    public void SaveNewTime(float newTime)
    {
        // Agregar el nuevo tiempo y ordenar la lista
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

        // Guardar los mejores tiempos
        SaveBestTimes();
    }

    public float[] GetBestTimes()
    {
        return bestTimes;
    }

    private void SaveBestTimes()
    {
        for (int i = 0; i < bestTimes.Length; i++)
        {
            PlayerPrefs.SetFloat($"BestTime{i}", bestTimes[i]);
        }
        PlayerPrefs.Save();
    }

    private void LoadBestTimes()
    {
        for (int i = 0; i < bestTimes.Length; i++)
        {
            bestTimes[i] = PlayerPrefs.GetFloat($"BestTime{i}", 0f);
        }
    }
}
