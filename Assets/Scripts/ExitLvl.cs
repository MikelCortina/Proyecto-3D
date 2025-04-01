using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int killCount = 0;
    public EndLevelUI end;
    public int killCountObjc;
    public GameObject startPanel; // Panel que se activará al cargar la escena
    public bool playable = false;
    public BestTimesManager bestTimesManager;
    public EndLevelUI endLevel;
    public TextMeshProUGUI bestTimesText;

    private void Start()
    {
        // Verificar si es la primera vez que se carga la escena
        if (!PlayerPrefs.HasKey("HasStarted_" + SceneManager.GetActiveScene().name))
        {
            PlayerPrefs.SetInt("HasStarted_" + SceneManager.GetActiveScene().name, 1);
            PlayerPrefs.Save();

            if (startPanel != null)
            {
                startPanel.SetActive(true);
                Time.timeScale = 0f;
                playable = false;
                string bestTimesDisplay = "";
                float[] bestTimes = bestTimesManager.GetBestTimes(endLevel.level);
                for (int i = 0; i < bestTimes.Length; i++)
                {
                    bestTimesDisplay += $"{i + 1}: {bestTimes[i]:F2} s\n";
                }
                bestTimesText.text = bestTimesDisplay;
            }
        }
        else
        {
            // Si la escena ya se ha cargado antes, desactivar el panel y permitir jugar
            startPanel.SetActive(false);
            playable = true;
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerPrefs.DeleteKey("HasStarted_" + SceneManager.GetActiveScene().name); // Restablecer solo esta escena
            ReloadScene();
            if (startPanel != null)
            {
               
                playable = false;
                startPanel.SetActive(true);
                Time.timeScale = 0f;

                string bestTimesDisplay = "";
                float[] bestTimes = bestTimesManager.GetBestTimes(endLevel.level);
                for (int i = 0; i < bestTimes.Length; i++)
                {
                    bestTimesDisplay += $"{i + 1}: {bestTimes[i]:F2} s\n";
                }
                bestTimesText.text = bestTimesDisplay;
            }
            // Desactivar el panel al presionar la tecla V
            if (Input.GetKeyDown(KeyCode.F) && startPanel.activeSelf)
            {
                PlayerPrefs.SetInt("HasStarted_" + SceneManager.GetActiveScene().name, 1);
                PlayerPrefs.Save();

                startPanel.SetActive(false);
                playable = true;
                Time.timeScale = 1f;
               
            }

            if (!playable && Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("MainMenu");
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ReloadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && killCount >= killCountObjc)
        {
            end.ShowEndScreen();
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

