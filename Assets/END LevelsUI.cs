using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    public GameObject endLevelPanel;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI bestTimesText;
    public Timer timer;
    public BestTimesManager bestTimesManager;

    void Start()
    {
        endLevelPanel.SetActive(false); // Oculta el panel al inicio
    }

    public void ShowEndScreen()
    {
        // Pausar el tiempo del juego
        Time.timeScale = 0f;

        // Mostrar y desbloquear el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        float finalTime = timer.GetCurrentTime();
        bestTimesManager.SaveNewTime(finalTime);

        finalTimeText.text = $" {finalTime:F2} s";

        string bestTimesDisplay = "";
        float[] bestTimes = bestTimesManager.GetBestTimes();
        for (int i = 0; i < bestTimes.Length; i++)
        {
            bestTimesDisplay += $"{i + 1}: {bestTimes[i]:F2} s\n";
        }
        bestTimesText.text = bestTimesDisplay;

        endLevelPanel.SetActive(true);
    }


    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lvl 1");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lvl2");
    }

}
