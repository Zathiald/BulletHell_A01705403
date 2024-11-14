using System.Collections;
using UnityEngine;

public class WhiplashBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;                // Velocidad de movimiento hacia adelante
    public float waveAmplitude = 1f;         // Amplitud de la onda (altitud máxima de la montaña)
    public float waveFrequency = 2f;         // Frecuencia de la onda (cuánto tiempo tarda en completar un ciclo)
    public float timeDestroy = 3f;           // Tiempo después del cual destruir la bala

    private float elapsedTime = 0f;          // Tiempo transcurrido para el movimiento de la onda
    public float damage = 10f; // Daño que causa la bala

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
            // Esto hará que la bala se mueva hacia arriba y abajo como "montañas"
            float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;

            // Aplica el movimiento hacia adelante (en Z) y el movimiento ondulado en Y
            rb.velocity = new Vector3(0, yOffset, -speed);  // Dirección de movimiento en 3D

            // Destruye la bala después de un tiempo determinado
            Destroy(gameObject, timeDestroy);
        }
        else
        {
            Debug.LogError("Rigidbody no encontrado en la bala.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión del enemigo con: " + other.gameObject.name);  // Ver qué objeto está colisionando
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colisión con jugador"); 
            var player = other.GetComponent<ShipController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Atacado Jugador");
            }
            Destroy(gameObject); // Destruye la bala al impactar
        }
    }
}
