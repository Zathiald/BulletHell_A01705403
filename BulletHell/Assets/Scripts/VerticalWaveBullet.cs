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
        if (rb == null)
        {
            Debug.LogError("Rigidbody no encontrado en la bala.");
            return;
        }
        // Programa la destrucción de la bala después de un tiempo
        Destroy(gameObject, timeDestroy);
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        // Incrementa el tiempo transcurrido
        elapsedTime += Time.fixedDeltaTime;

        // Calcula el desplazamiento vertical (movimiento ondulatorio)
        float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;

        // Aplica el movimiento hacia adelante y el desplazamiento ondulatorio
        Vector3 forwardMovement = transform.forward * speed; // Usa transform.forward para el movimiento hacia adelante
        Vector3 verticalMovement = new Vector3(0f, yOffset, 0f);

        rb.velocity = forwardMovement + verticalMovement;
    }

    void OnTriggerEnter(Collider other)
    {
        // Maneja la colisión con el jugador
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<ShipController>();
            if (player != null)
            {
                player.TakeDamage(damage, Color.blue);
            }

            Destroy(gameObject); // Destruye la bala al impactar
        }
    }
}
