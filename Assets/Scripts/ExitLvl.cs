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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&killCount==killCountObjc) 
        {
            
        end.ShowEndScreen();
        }
    }
}