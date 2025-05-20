// Añade este script a tu GameObject con AudioSource
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
