using System.Collections;
using UnityEngine;

public class OvniShooter : MonoBehaviour, IDamage
{
    public float circleRadius = 5f;      // Radio del movimiento circular
    public float circleSpeed = 1f;       // Velocidad del movimiento circular
    public float rotationSpeed = 50f;    // Velocidad de rotación del objeto
    public GameObject bulletPrefab;      // Prefab del proyectil
    public float fireRate = 1f;          // Intervalo de disparo en segundos
    public float projectileSpeed = 10f;  // Velocidad del proyectil
    public Transform[] firePoints;       // Puntos desde los cuales disparar

    private float angle = 0f;
    private int currentFirePointIndex = 0; // Índice actual del punto de disparo
    public float health = 50f; // Vida del enemigo
    private Renderer enemyRenderer;  // Para acceder al Renderer del objeto
    private Color originalColor; // Para guardar el color de emisión original

    public AudioClip hitSound;
    private AudioSource audioSource;
    public EnemyManager enemyManager;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();  // Obtener el Renderer del objeto
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.GetColor("_Color");  // Guardar el color original
        }

        StartCoroutine(AutoShoot()); // Inicia la rutina de disparo

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        MoveInCircle();
        SpinObject();
    }

    void MoveInCircle()
    {
        angle += circleSpeed * Time.deltaTime; // Incrementa el ángulo
        float x = Mathf.Cos(angle) * circleRadius;
        float z = Mathf.Sin(angle) * circleRadius;
        transform.position = new Vector3(x, transform.position.y, z);
    }

    void SpinObject()
    {
        // Rota el objeto sobre su propio eje (como un beyblade)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        // Dispara desde el punto de disparo actual
        Transform firePoint = firePoints[currentFirePointIndex];
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Avanza al siguiente punto de disparo en orden
        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemigo dañado. Vida restante: " + health);

        // Cambiar el color a rojo al recibir daño
        if (enemyRenderer != null)
        {
            enemyRenderer.material.SetColor("_Color", Color.red);  // Cambia el color a rojo
            StartCoroutine(ResetColor()); // Llama a la corutina para restaurar el color después de un tiempo
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
        // Espera 0.5 segundos
        yield return new WaitForSeconds(0.5f);

        // Restaurar el color original del material
        if (enemyRenderer != null)
        {
            enemyRenderer.material.SetColor("_Color", originalColor);
        }
    }

    void Die()
    {
        Debug.Log("Enemigo destruido");
        enemyManager.OnEnemyDestroyed(gameObject);
        Destroy(gameObject);
    }
}
