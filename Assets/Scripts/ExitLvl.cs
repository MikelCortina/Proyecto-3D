using LootLocker.Requests;
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
    public int level;
    public LeaderboardUploader leaderboardUploader;
    public TextMeshProUGUI onlineLeaderboardText; // Asignalo desde el inspector
    public bool tutorial;
    public int tutolvl;

    private void Start()
    {
        // Verificar si es la primera vez que se carga la escena
        if (!PlayerPrefs.HasKey("HasStarted_" + SceneManager.GetActiveScene().name)&&!tutorial)
        {
            PlayerPrefs.SetInt("HasStarted_" + SceneManager.GetActiveScene().name, 1);
            PlayerPrefs.Save();

            if (startPanel != null)
            {
                startPanel.SetActive(true);
                Time.timeScale = 0f;
                playable = false;
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
       
        if (Input.GetKeyDown(KeyCode.G)&&!tutorial)
        {
            PlayerPrefs.DeleteKey("HasStarted_" + SceneManager.GetActiveScene().name); // Restablecer solo esta escena
            ReloadScene();
            if (startPanel != null)
            {
               
                playable = false;
                startPanel.SetActive(true);
                Time.timeScale = 0f;

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
        if (other.CompareTag("Player") && killCount >= killCountObjc &&!tutorial)
        {
            end.ShowEndScreen();
        }
        if (other.CompareTag("Player") && killCount >= killCountObjc && tutorial&& tutolvl!=6)
        {
            SceneManager.LoadScene("Tutorial"+tutolvl);
        }
        if (other.CompareTag("Player") && killCount >= killCountObjc && tutorial&& tutolvl==6)
        {
            end.ShowPanel();
        }

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

