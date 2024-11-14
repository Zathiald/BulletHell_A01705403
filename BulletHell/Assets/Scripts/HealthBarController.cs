using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider healthSlider; // Asigna aquí el Slider en el Inspector
    public Image healthFillImage; // Asigna aquí la imagen de relleno del slider en el Inspector
    public float maxHealth = 120f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; // Asegura que empiece completamente relleno
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth; // Ajusta el valor del Slider basado en la vida actual

        // Cambiar el color de la barra de vida según el porcentaje de salud
        float healthPercentage = currentHealth / maxHealth;

        if (healthPercentage > 0.5f)
        {
            // Azul verdoso cuando la vida está por encima del 50%
            healthFillImage.color = Color.Lerp(new Color(0.0f, 0.5f, 0.5f), Color.cyan, (healthPercentage - 0.5f) * 2);
        }
        else if (healthPercentage > 0.25f)
        {
            // Amarillo cuando la vida está entre el 25% y el 50%
            healthFillImage.color = Color.Lerp(Color.yellow, new Color(0.5f, 1.0f, 0.0f), (healthPercentage - 0.25f) * 4);
        }
        else
        {
            // Rojo cuando la vida está por debajo del 25%
            healthFillImage.color = Color.Lerp(Color.red, Color.yellow, healthPercentage * 4);
        }

        // Invertir la escala verticalmente para que se vacíe hacia abajo
        RectTransform fillRect = healthFillImage.GetComponent<RectTransform>();
        fillRect.localScale = new Vector3(fillRect.localScale.x, healthPercentage, fillRect.localScale.z);
    }
}




