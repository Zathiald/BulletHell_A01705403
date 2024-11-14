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
        // Cambiar el valor del Slider basándonos en la vida actual
        healthSlider.value = currentHealth / maxHealth;

        // Obtener el porcentaje de salud actual
        float healthPercentage = currentHealth / maxHealth;

        // Cambiar el color de la barra de vida según el porcentaje de salud
        if (healthPercentage > 0.5f)
        {
            // Azul verdoso cuando la vida está por encima del 50%
            healthFillImage.color = Color.Lerp(Color.green, Color.cyan, 1 - (healthPercentage - 0.5f) * 2);
        }
        else if (healthPercentage > 0.2f)
        {
            // Amarillo cuando la vida está entre el 50% y el 20%
            healthFillImage.color = Color.Lerp(Color.green, Color.yellow, (0.5f - healthPercentage) * 2);
        }
        else
        {
            // Rojo cuando la vida está por debajo del 20%
            healthFillImage.color = Color.Lerp(Color.yellow, Color.red, (0.2f - healthPercentage) * 5);
        }

        // Invertir la escala del relleno para que se llene hacia abajo
        RectTransform fillRect = healthFillImage.GetComponent<RectTransform>();
        fillRect.localScale = new Vector3(fillRect.localScale.x, -healthSlider.value, fillRect.localScale.z);
    }
}



