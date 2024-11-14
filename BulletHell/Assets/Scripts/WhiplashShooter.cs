using System.Collections;
using UnityEngine;

public class WhiplashShooter : MonoBehaviour, IDamage
{
    public Transform[] pathPoints;  // Arreglo de puntos por los cuales se moverá la nave
    public float moveSpeed = 5f;    // Velocidad de movimiento de la nave
    public GameObject bulletPrefab; // Prefab del proyectil
    public float fireRate = 0.5f;   // Intervalo de disparo en segundos
    public Transform[] firePoints;  // Puntos desde los cuales disparar

    private int currentPointIndex = 0; // Índice del punto actual en el arreglo
    private float t = 0f;               // Variable para interpolar entre los puntos
    public float health = 50f;          // Vida del enemigo
    private Renderer enemyRenderer;     // Para acceder al Renderer del objeto
    public Material originalMaterial;   // Para guardar el material original

    public AudioClip hitSound;
    private AudioSource audioSource;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        StartCoroutine(AutoShoot()); // Inicia la rutina de disparo automático

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        MoveShip();  // Llama a la función para mover la nave
    }

    void MoveShip()
    {
        // Asegúrate de que hay al menos 2 puntos en el arreglo para moverse
        if (pathPoints.Length < 2) return;

        // Punto actual y siguiente en el camino
        Transform startPoint = pathPoints[currentPointIndex];
        Transform endPoint = pathPoints[(currentPointIndex + 1) % pathPoints.Length];

        // Interpola entre los puntos actuales
        t += Time.deltaTime * moveSpeed;

        if (t > 1f)  // Si ha llegado al final del punto actual, pasa al siguiente punto
        {
            t = 0f; // Reinicia t para el próximo segmento
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Length; // Avanza al siguiente punto
        }

        // Actualiza la posición de la nave entre los puntos usando Lerp
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate); // Espera entre disparos
            ShootProjectile(); // Dispara un proyectil
        }
    }

    void ShootProjectile()
    {
        // Dispara un proyectil desde cada punto de disparo
        foreach (Transform firePoint in firePoints)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemigo Whiplash dañado. Vida restante: " + health);

        // Cambiar el color a rojo al recibir daño
        if (enemyRenderer != null)
        {
            // Cambiar el color del material a rojo
            enemyRenderer.material.color = Color.red;

            // Llamar a la corutina para restaurar el color después de 1 segundo
            StartCoroutine(ResetColor());
        }

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator ResetColor()
    {
        // Espera 1 segundo
        yield return new WaitForSeconds(0.5f);

        // Restaurar el material original
        if (enemyRenderer != null && originalMaterial != null)
        {
            // Restaurar el color original del material
            enemyRenderer.material = originalMaterial;
        }
    }

    void Die()
    {
        Debug.Log("Enemigo destruido");
        Destroy(gameObject);
    }
}
