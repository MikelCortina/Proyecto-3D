using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    // Función para iniciar el juego
    public void Menu()
    {
        SceneManager.LoadScene("Menu Jugar"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    public void MainMenu()
    {
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

    public void Historia()
    {
        SceneManager.LoadScene("Lvl 1"); // Cambia "GameScene" por el nombre de tu escena de juego
    }

    public void DEMO()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    
    public void Config()
    {
        SceneManager.LoadScene("Config"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    




    // Función para salir del juego
    public void ExitGame()
    {
        Application.Quit();
    }
}
