using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Función para iniciar el juego
    public void SampleScene()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia "GameScene" por el nombre de tu escena de juego
    }
    public void Creditos()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia "GameScene" por el nombre de tu escena de juego
    }

    // Función para salir del juego
    public void ExitGame()
    {
        Application.Quit();
    }
}
