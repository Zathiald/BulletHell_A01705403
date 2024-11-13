using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceShip : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float forwardSpeed = 25f, strafeSpeed = 10f, hoverSpeed = 5f;
    public float lookRateSpeed = 90f;
    public float followDistance = 20f; // Distancia a la que la nave detecta al jugador
    public float randomMoveSpeed = 5f; // Velocidad del movimiento aleatorio
    public float randomMoveInterval = 3f; // Intervalo de tiempo entre movimientos aleatorios
    public float areaRange = 50f; // Rango del área donde puede moverse aleatoriamente
    private Vector3 randomTargetPosition; // Objetivo aleatorio para moverse
    private float timeSinceLastRandomMove = 0f;

    // Variables de disparo
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform[] firePoints; // Puntos de disparo
    public float fireRate = 1f; // Tiempo entre disparos (en segundos)
    private float fireCooldown = 0f; // Temporizador para controlar la frecuencia de disparo

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform; // Asignar al jugador si no está asignado
            SetRandomTargetPosition(); // Establecer un objetivo aleatorio al inicio
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Calcular la distancia entre la nave y el jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < followDistance)
            {
                // La nave sigue al jugador si está cerca
                FollowPlayer();
            }
            else
            {
                // Si está lejos, realiza un movimiento aleatorio
                MoveRandomly();
            }
        }

        // Controla el disparo y el tiempo de espera entre ráfagas
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            FireAllBullets();
            fireCooldown = fireRate;  // Reinicia el temporizador
        }
    }

    void FollowPlayer()
    {
        // Mueve la nave enemiga hacia el jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.position += directionToPlayer * forwardSpeed * Time.deltaTime;

        // Gira la nave para mirar hacia el jugador
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, lookRateSpeed * Time.deltaTime);
    }

    void MoveRandomly()
    {
        // Realiza un movimiento aleatorio dentro del área determinada
        timeSinceLastRandomMove += Time.deltaTime;

        if (timeSinceLastRandomMove >= randomMoveInterval)
        {
            SetRandomTargetPosition();
            timeSinceLastRandomMove = 0f;
        }

        // Mueve la nave hacia el objetivo aleatorio
        Vector3 directionToRandomTarget = (randomTargetPosition - transform.position).normalized;
        transform.position += directionToRandomTarget * randomMoveSpeed * Time.deltaTime;

        // Gira la nave para mirar hacia el objetivo aleatorio
        Quaternion rotation = Quaternion.LookRotation(directionToRandomTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, lookRateSpeed * Time.deltaTime);
    }

    void SetRandomTargetPosition()
    {
        // Establece un nuevo objetivo aleatorio dentro de un área determinada
        randomTargetPosition = new Vector3(
            Random.Range(-areaRange, areaRange),
            Random.Range(-areaRange, areaRange),
            Random.Range(-areaRange, areaRange)
        );
    }

    void FireAllBullets()
    {
        foreach (Transform firePoint in firePoints)
        {
            // Asegúrate de que las balas se disparen hacia adelante
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}

