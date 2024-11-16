using System.Collections;
using UnityEngine;

public class HorizontalWaveBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;                // Velocidad de movimiento hacia adelante
    public float waveAmplitude = 1f;         // Amplitud de la onda (desplazamiento horizontal)
    public float waveFrequency = 2f;         // Frecuencia de la onda (velocidad del ciclo)
    public float timeDestroy = 3f;           // Tiempo de vida de la bala
    public float damage = 10f;               // Daño que causa la bala

    private float elapsedTime = 0f;          // Tiempo acumulado desde que la bala se generó

    void Start()
    {
        // Inicializa el Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Incrementa el tiempo transcurrido
            elapsedTime += Time.fixedDeltaTime;

            // Calcula el desplazamiento horizontal (movimiento ondulatorio)
            float xOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;

            // Aplica el movimiento hacia adelante y el desplazamiento ondulatorio en X
            Vector3 forwardMovement = transform.forward * speed; // Usa transform.forward para avanzar
            Vector3 horizontalMovement = transform.right * xOffset; // Usa transform.right para el movimiento en X

            rb.velocity = forwardMovement + horizontalMovement;
            // Programa la destrucción de la bala después de un tiempo
            Destroy(gameObject, timeDestroy);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Maneja la colisión con el jugador
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<ShipController>();
            if (player != null)
            {
                player.TakeDamage(damage, new Color(1f, 0.5f, 0f));
            }

            Destroy(gameObject); // Destruye la bala al impactar
        }
    }
}
