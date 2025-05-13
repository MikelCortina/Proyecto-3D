using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FirstTimeIntro : MonoBehaviour
{
    public float blinkInterval = 0.5f;

    private List<Image> blinkingImages = new List<Image>();
    private bool isFirstTime = false;
    private float timer = 0f;
    private string nickname;

    void Start()
    {
        nickname = PlayerPrefs.GetString("PlayerName", "");

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("No hay nombre de jugador guardado.");
            gameObject.SetActive(false);
            return;
        }

        isFirstTime = !PlayerPrefs.HasKey("User_" + nickname);

        if (isFirstTime)
        {
            Image[] allImages = FindObjectsOfType<Image>();
            foreach (Image img in allImages)
            {
                if (img.CompareTag("Parpadeo"))
                    blinkingImages.Add(img);
            }

            foreach (Image img in blinkingImages)
                img.enabled = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isFirstTime) return;

        timer += Time.deltaTime;
        if (timer >= blinkInterval)
        {
            foreach (Image img in blinkingImages)
                img.enabled = !img.enabled;
            timer = 0f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlayerPrefs.SetInt("User_" + nickname, 1);
            PlayerPrefs.Save();

            foreach (Image img in blinkingImages)
                img.enabled = false;

            enabled = false;
        }
    }
}
