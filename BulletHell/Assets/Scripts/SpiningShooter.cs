using System.Collections;
using UnityEngine;

public class SpiningShooter : MonoBehaviour, IDamage
{
    public float rotationSpeed = 50f;
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public Transform[] firePoints;
    public float health = 50f; // Vida del enemigo

    private float angle = 0f;
    private int currentFirePointIndex = 0;
    private Renderer enemyRenderer;  // Para acceder al Renderer del objeto
    public Material originalMaterial; // Para guardar el material original

    public AudioClip hitSound;
    private AudioSource audioSource;
    public EnemyManager enemyManager;

    void Start()
    {
        // Obtener el componente Renderer del objeto
        enemyRenderer = GetComponent<Renderer>();
        StartCoroutine(AutoShoot());

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        SpinObject();
    }

    void SpinObject()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.World);
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
        Transform firePoint = firePoints[currentFirePointIndex];
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemigo dañado. Vida restante: " + health);

        // Cambiar el color a rojo al recibir daño
        if (enemyRenderer != null && enemyRenderer.materials.Length > 1)
        {
            // Cambiar el color del segundo material a rojo
            enemyRenderer.materials[1].SetColor("_Color", Color.red);

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

        // Restaurar el material original del segundo material
        if (enemyRenderer != null && originalMaterial != null)
        {
            // Restaurar el material original (si lo hemos guardado)
            Material[] materials = enemyRenderer.materials; // Obtener los materiales actuales
            materials[1] = originalMaterial; // Cambiar el material en el índice 1
            enemyRenderer.materials = materials; // Asignar de nuevo el arreglo de materiales
        }
    }


    void Die()
    {
        Debug.Log("Enemigo destruido");
        enemyManager.OnEnemyDestroyed(gameObject);
        Destroy(gameObject);
    }
}

