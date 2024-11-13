using System.Collections;
using UnityEngine;

public class WhiplashBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;                // Velocidad de movimiento hacia adelante
    public float waveAmplitude = 1f;         // Amplitud de la onda
    public float waveFrequency = 2f;         // Frecuencia de la onda
    public float timeDestroy = 3f;           // Tiempo después del cual destruir la bala

    private float elapsedTime = 0f;          // Tiempo transcurrido para el movimiento de la onda

    void Start()
    {
        transform.Rotate(90f, 0f, 0f);      // Asegura que la bala esté orientada correctamente
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb != null)
        {
            // Actualiza el tiempo transcurrido para el movimiento en onda
            elapsedTime += Time.deltaTime;

            // Calcula el desplazamiento vertical con la función seno (onda)
            float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;

            // Aplica el movimiento hacia adelante (en Z) y el movimiento ondulado en Y
            rb.velocity = new Vector3(0, yOffset, speed);  // Dirección de movimiento en 3D

            // Destruye la bala después de un tiempo determinado
            Destroy(gameObject, timeDestroy);
        }
        else
        {
            Debug.LogError("Rigidbody no encontrado en la bala.");
        }
    }
}
