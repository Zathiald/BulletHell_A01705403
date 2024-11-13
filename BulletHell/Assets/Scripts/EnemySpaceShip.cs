using System.Collections;
using UnityEngine;

public class EnemySpaceShip : MonoBehaviour
{
    public float moveSpeed = 5f;       // Velocidad de movimiento de la nave
    public float rangeX = 5f;          // Rango de movimiento horizontal (izquierda/derecha)
    public GameObject bulletPrefab;    // Prefab del proyectil
    public float fireRate = 0.5f;      // Intervalo de disparo en segundos
    public float waveFrequency = 1f;   // Frecuencia de las ondas de los proyectiles
    public float waveAmplitude = 1f;   // Amplitud de las ondas (cuánto sube y baja el proyectil)
    public Transform[] firePoints;     // Puntos de disparo

    void Start()
    {
        StartCoroutine(AutoShoot()); // Inicia la rutina de disparo
    }

    void Update()
    {
        HandleMovement(); // Mueve la nave
    }

    void HandleMovement()
    {
        // Movimiento horizontal basado en la entrada del jugador
        float moveInput = Input.GetAxis("Horizontal");  // Utiliza las teclas A/D o flechas izquierda/derecha
        Vector3 moveDirection = new Vector3(moveInput, 0, 0);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Limita el movimiento de la nave dentro del rango especificado
        float clampedX = Mathf.Clamp(transform.position.x, -rangeX, rangeX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate); // Espera el intervalo de disparo
            ShootProjectile(); // Dispara los proyectiles
        }
    }

    void ShootProjectile()
    {
        // Dispara desde todos los puntos de disparo al mismo tiempo
        foreach (Transform firePoint in firePoints)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}

