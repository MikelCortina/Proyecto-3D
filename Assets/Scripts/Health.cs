using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Para manejar imágenes de UI

public class Health : MonoBehaviour
{
    public int amount = 3;
    public Image[] healthImages; // Array de imágenes de vida
    private DashRigidbody dash;

    void Start()
    {
        dash = GetComponent<DashRigidbody>();
        amount = healthImages.Length; // Asegurar que la cantidad de vida coincide con las imágenes
    }

    void Update()
    {
        if (amount <= 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void TakeDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (amount > 0)
            {
                amount--;
                healthImages[amount].gameObject.SetActive(false); // Desactiva la imagen de la vida correspondiente
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BalaEnemy") && !dash.isDashing)
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
