using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using LootLocker.Requests;

public class EndLevelUI : MonoBehaviour
{
    public GameObject endLevelPanel;
    public GameObject endLevelPanel2;
    public TextMeshProUGUI finalTimeText;
    public Timer timer;
    public BestTimesManager bestTimesManager;
    public int level;
    public LevelManager levelManager;
    public LeaderboardUploader leaderboardUploader;
    public TextMeshProUGUI onlineLeaderboardText; // Asignalo desde el inspector
    void Start()
    {
        endLevelPanel.SetActive(false); // Oculta el panel al inicio
        if (leaderboardUploader == null)
        {
            leaderboardUploader = FindObjectOfType<LeaderboardUploader>();
        }
        string leaderboardKey = "level" + level;
        leaderboardUploader.leaderboardKey = leaderboardKey;
    }
    public void ShowEndScreen()
    {
        levelManager.playable = false;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        float finalTime = timer.GetCurrentTime();
        bestTimesManager.SaveNewTime(finalTime, level);

        finalTimeText.text = $" Your Time: {finalTime:F2} s";

        /* Mostrar mejores tiempos locales
        string bestTimesDisplay = "";
        float[] bestTimes = bestTimesManager.GetBestTimes(level);
        for (int i = 0; i < bestTimes.Length; i++)
        {
            bestTimesDisplay += $"{i + 1}: {bestTimes[i]:F2} s\n";
        }
        bestTimesText.text = bestTimesDisplay;*/

        // Mostrar leaderboard online
        string leaderboardKey = "level" + level;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        { 
            if (response.success)
            {
                string leaderboardDisplay = "";
                for (int i = 0; i < response.items.Length; i++)
                {
                    string name = string.IsNullOrEmpty(response.items[i].member_id) ? "Jugador" : response.items[i].member_id;
                    float tiempo = response.items[i].score / 100f;
                    leaderboardDisplay += $"{i + 1}. {name}: {tiempo:F2} s\n";
                }
                onlineLeaderboardText.text = leaderboardDisplay;
            }
            else
            {
                onlineLeaderboardText.text = "Error al obtener leaderboard online.";
            }
        });

        endLevelPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ShowPanel()
    {
        levelManager.playable = false;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        endLevelPanel2.SetActive(true);
    }
    public void NextLevel()
    {
        PlayerPrefs.DeleteKey("HasStarted_" + SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1);
    }
}
