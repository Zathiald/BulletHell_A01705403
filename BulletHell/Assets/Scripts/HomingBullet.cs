using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;         // Velocidad de la bala
    public float timeDestroy = 3f;    // Tiempo antes de destruir la bala
    public float damage = 10f;        // Daño que causa la bala
    public Transform player;          // Referencia al jugador

    public float slowFactor = 0.5f;   // Factor de lentitud
    public float homingDeviation = 0.1f; // Variabilidad en la persecución de la bala (desviación aleatoria)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TryFindPlayer(); // Intentar encontrar al jugador al inicio
    }

    void TryFindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("[HomingBullet] No se encontró un objeto con la etiqueta 'Player', reintentando...");
            // Llamamos de nuevo después de un breve retraso
            Invoke(nameof(TryFindPlayer), 0.5f);  // Vuelve a intentar encontrar al jugador cada 0.5 segundos
        }
        else
        {
            Debug.Log("[HomingBullet] Jugador encontrado: " + player.name);
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Debug.Log("[HomingBullet] Persiguiendo al jugador...");
            transform.LookAt(player.transform);

            // Desviación aleatoria en la dirección de persecución
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 deviation = new Vector3(
                Random.Range(-homingDeviation, homingDeviation),  // Desviación en el eje X
                Random.Range(-homingDeviation, homingDeviation),  // Desviación en el eje Y
                Random.Range(-homingDeviation, homingDeviation)   // Desviación en el eje Z
            );

            // Aplicar la desviación en la dirección de la bala
            Vector3 finalDirection = (directionToPlayer + deviation).normalized;

            // Movimiento de la bala con la dirección final con desviación y velocidad
            transform.position += finalDirection * speed * slowFactor * Time.deltaTime;
        }
        else
        {
            Debug.LogWarning("[HomingBullet] No hay jugador. La bala seguirá recta.");
            rb.velocity = transform.forward * speed;
        }
        Destroy(gameObject, timeDestroy);
    }

    IEnumerator ShootAtPlayer()
    {
        Debug.Log("[HomingBullet] Iniciando persecución del jugador...");
        while (player != null)
        {
            if (player == null)
            {
                Debug.LogWarning("[HomingBullet] El jugador ya no existe. Finalizando corrutina.");
                yield break;
            }

            // Desviación en la trayectoria para hacer que sea más difícil para la bala seguir al jugador
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 deviation = new Vector3(
                Random.Range(-homingDeviation, homingDeviation),
                Random.Range(-homingDeviation, homingDeviation),
                Random.Range(-homingDeviation, homingDeviation)
            );

            Vector3 finalDirection = (directionToPlayer + deviation).normalized;

            transform.position += finalDirection * speed * slowFactor * Time.deltaTime;
            transform.LookAt(player);
            yield return null;
        }

        Debug.Log("[HomingBullet] Persecución terminada. Bala destruida en " + timeDestroy + " segundos.");
        Destroy(gameObject, timeDestroy);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[HomingBullet] Colisión con: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("[HomingBullet] Colisión con el jugador detectada.");
            var playerController = other.GetComponent<ShipController>();
            if (playerController != null)
            {
                Debug.Log("[HomingBullet] Aplicando daño al jugador: " + damage);
                //playerController.TakeDamage(damage, new Color(1f, 0.5f, 0f));
            }
            Destroy(gameObject);
        }
    }
}
