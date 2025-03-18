using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Agrega esto para usar UI Text

public class Health : MonoBehaviour
{
    public int amount = 3;
    public TextMeshProUGUI healthText; // Referencia al texto de la UI
    private DashRigidbody dash;

    void Start()
    {
        dash = GetComponent<DashRigidbody>();
        UpdateHealthUI(); // Mostrar la vida al iniciar
         amount = 3;
}

    void Update()
    {
        if (amount <= 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        UpdateHealthUI(); // Actualiza la UI cada frame (opcional, depende de si cambias vida aquí o en otros scripts)
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + amount.ToString();
        }
    }

    // Ejemplo de función para reducir vida desde otros scripts
    public void TakeDamage(int damage)
    {
        amount -= damage;
        UpdateHealthUI();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BalaEnemy")&&!dash.isDashing)
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
