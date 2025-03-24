using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int killCount = 0;
    public EndLevelUI end;
    public int killCountObjc;
    public GameObject startPanel; // Panel que se activará al cargar la escena
    public bool playable=false;
    public BestTimesManager bestTimesManager;
    public EndLevelUI endLevel;
    public TextMeshProUGUI bestTimesText;


    private void Start()
    {
        // Activar el panel al inicio
        if (startPanel != null)
        {
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
    }

    private void Update()
    {
        // Desactivar el panel al presionar la barra espaciadora
        if (Input.GetKeyDown(KeyCode.V) && startPanel.activeSelf)
        {
            startPanel.SetActive(false);
            playable = true;
            Time.timeScale = 1f;
        }
        if (!playable && Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MenuPrincipal"); // Cambia "GameScene" por el nombre de tu escena de juego
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMenu");
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

