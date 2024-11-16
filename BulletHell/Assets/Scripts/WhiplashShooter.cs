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
    public float health = 50f;         // Vida del enemigo
    private Renderer enemyRenderer;    // Para acceder al Renderer del objeto
    public Material originalMaterial;  // Para guardar el material original

    public AudioClip hitSound;
    private AudioSource audioSource;
    public EnemyManager enemyManager;

    void Start()
    {
        // Verifica que hay al menos dos puntos en el camino
        if (pathPoints.Length < 2)
        {
            StartCoroutine(AutoShoot()); // Inicia la rutina de disparo automático
        }

        // Establece la posición inicial de la nave en el primer punto
        transform.position = pathPoints[0].position;

        enemyRenderer = GetComponent<Renderer>();
        StartCoroutine(AutoShoot()); // Inicia la rutina de disparo automático

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (pathPoints.Length >= 2) // Asegura que hay suficientes puntos
        {
            MoveShip();
        }
    }

    void OnEnable()
    {
        StartCoroutine(AutoShoot()); // Inicia la rutina de disparo automático al activarse
    }

    void MoveShip()
    {
        // Punto actual y siguiente en el camino
        Transform endPoint = pathPoints[(currentPointIndex + 1) % pathPoints.Length];

        // Mueve la nave hacia el siguiente punto usando MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, moveSpeed * Time.deltaTime);

        // Si la nave ha llegado al punto final, avanza al siguiente punto
        if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Length; // Avanza al siguiente punto
        }
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
        enemyManager.OnEnemyDestroyed(gameObject);
        Destroy(gameObject);
    }
}
