using UnityEngine;

public class Glovo : MonoBehaviour
{
    public AudioClip destroySound;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float fuerza;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        audioSource.PlayOneShot(destroySound);
    }
}
