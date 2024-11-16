using System.Collections;
using UnityEngine;

public class VerticalWaveBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;                // Velocidad de la bala hacia adelante
    public float waveAmplitude = 1f;         // Altura máxima del movimiento ondulatorio
    public float waveFrequency = 2f;         // Frecuencia de la onda (velocidad del ciclo)
    public float timeDestroy = 3f;           // Tiempo de vida de la bala antes de destruirse
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

            // Calcula el desplazamiento vertical (movimiento ondulatorio)
            float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;

            // Aplica el movimiento hacia adelante y el desplazamiento ondulatorio
            Vector3 forwardMovement = transform.forward * speed; // Usa transform.forward para el movimiento hacia adelante
            Vector3 verticalMovement = new Vector3(0f, yOffset, 0f);

            rb.velocity = forwardMovement + verticalMovement;
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
