using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int killCount = 0;
    public EndLevelUI end;
    public int killCountObjc;
    public GameObject startPanel; // Panel que se activará al cargar la escena
    public bool playable=false;


    private void Start()
    {
        // Activar el panel al inicio
        if (startPanel != null)
        {
            startPanel.SetActive(true);
            Time.timeScale = 0f;
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

