using System.Collections;
using UnityEngine;

public class WhiplashShooter : MonoBehaviour, IDamage
{
    public Transform startPoint;    // Punto de inicio de la nave
    public Transform endPoint;      // Punto final de la nave
    public float moveSpeed = 5f;    // Velocidad de movimiento de la nave
    public GameObject bulletPrefab; // Prefab del proyectil
    public float fireRate = 0.5f;   // Intervalo de disparo en segundos
    public Transform[] firePoints;  // Puntos desde los cuales disparar

    private float t = 0f;           // Variable para interpolar entre los dos puntos
    public float health = 50f; // Vida del enemigo
    private Renderer enemyRenderer;  // Para acceder al Renderer del objeto
    public Material originalMaterial; // Para guardar el material original

    void Start()
    {
        StartCoroutine(AutoShoot()); // Inicia la rutina de disparo automático
    }

    void Update()
    {
        MoveShip();  // Llama a la función para mover la nave
    }

    void MoveShip()
    {
        // Interpola entre los puntos startPoint y endPoint
        t += Time.deltaTime * moveSpeed;

        if (t > 1f)  // Si ha llegado al final, invierte la dirección
        {
            t = 0f; // Vuelve al principio
            // Intercambia los puntos de inicio y fin
            var temp = startPoint;
            startPoint = endPoint;
            endPoint = temp;
        }

        // Actualiza la posición de la nave entre los dos puntos usando Lerp
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
        Debug.Log("Enemigo dañado. Vida restante: " + health);

        // Cambiar el color a rojo al recibir daño
        if (enemyRenderer != null)
        {
            // Cambiar el color del material a rojo
            enemyRenderer.material.color = Color.red;

            // Llamar a la corutina para restaurar el color después de 1 segundo
            StartCoroutine(ResetColor());
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
