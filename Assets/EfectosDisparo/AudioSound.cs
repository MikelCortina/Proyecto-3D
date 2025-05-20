using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    private AudioSource musicSource;

    void Start()
    {
        GameObject musicObj = GameObject.Find("MusicPlayer"); // asegúrate que el nombre es correcto
        if (musicObj != null)
        {
            musicSource = musicObj.GetComponent<AudioSource>();
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto MusicPlayer");
        }
    }

    public void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;

            // Fuerza a detener el audio si está en 0
            if (volume <= 0.0001f && musicSource.isPlaying)
            {
                musicSource.Pause(); // o Stop() si prefieres reiniciar la música
            }
            else if (volume > 0 && !musicSource.isPlaying)
            {
                musicSource.UnPause(); // o Play() si usaste Stop()
            }
        }
    }

}
