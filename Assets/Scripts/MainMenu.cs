using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    // Función para iniciar el juego
    public void Menu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu Jugar"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    public void MainMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MenuPrincipal"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    public void Creditos()
    {

        SceneManager.LoadScene("SampleScene"); // Cambia "GameScene" por el nombre de tu escena de juego
    }

    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    public void Sensi()
    {
        SceneManager.LoadScene("Sensibilidad");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial"); 
    }

    public void Historia()
    {
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl1");
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl2");
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl3");
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl4");
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl5");
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl6");
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl7");
        PlayerPrefs.DeleteKey("HasStarted_" + "Lvl8");
        SceneManager.LoadScene("Lvl 1"); // Cambia "GameScene" por el nombre de tu escena de juego
    }

    public void DEMO()
    {
        SceneManager.LoadScene("Tutorial"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    
    public void Config()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Config"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    




    // Función para salir del juego
    public void ExitGame()
    {
        Application.Quit();
    }
}
