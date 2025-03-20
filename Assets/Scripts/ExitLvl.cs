using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public int killCount = 0;
    public EndLevelUI end;
    public int killCountObjc;

    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 1f;
            ReloadScene();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&killCount>=killCountObjc) 
        {
            
        end.ShowEndScreen();
        }
    }
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}