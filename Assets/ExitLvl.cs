using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public int killCount = 0;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&killCount==1) 
        {
            
        SceneManager.LoadScene("Menu Jugar");
        }
    }
}