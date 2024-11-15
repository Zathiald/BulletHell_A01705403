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

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth; // Ajusta el valor del Slider basado en la vida actual

        // Cambiar el color de la barra de vida según el porcentaje de salud
        float healthPercentage = currentHealth / maxHealth;

        if ( healthPercentage < 0.5f)
        {
            // Amarillo cuando la vida está entre el 25% y el 50%
            healthFillImage.color = Color.yellow;
        }
        if (healthPercentage < 0.25f)
        {
            // Rojo cuando la vida está por debajo del 25%
            healthFillImage.color = Color.red;
        }

        // Invertir la escala verticalmente para que se vacíe hacia abajo
        RectTransform fillRect = healthFillImage.GetComponent<RectTransform>();
        fillRect.localScale = new Vector3(fillRect.localScale.x, healthPercentage, fillRect.localScale.z);
    }
}




