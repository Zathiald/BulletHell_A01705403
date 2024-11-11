using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCreator : MonoBehaviour
{
    [SerializeField] GameObject asteroid;
    public Transform player;
    public Camera playerCamera;
    public float spawnDistance = 10.0f;
    public float minDistanceFromPlayer = 5.0f; // Distancia mínima desde el jugador

    void Start()
    {
        InvokeRepeating("InstantiateAsteroid", 0, 2); // Esto creará un asteroide cada 2 segundos
    }

    void InstantiateAsteroid()
    {
        Vector3 spawnDirection = Random.onUnitSphere;
        spawnDirection.y = 0;  // Esto asegura que los asteroides solo se generen en el plano horizontal

        Vector3 spawnPoint = player.position + spawnDirection * spawnDistance;

        // Verifica si el punto de generación está dentro del campo de visión del jugador
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(spawnPoint);
        if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
        {
            // Si el punto de generación está dentro del campo de visión del jugador, mueve el punto de generación
            spawnPoint = player.position - spawnDirection * spawnDistance;
        }

        // Verifica si el punto de generación está demasiado cerca del jugador
        if (Vector3.Distance(player.position, spawnPoint) < minDistanceFromPlayer)
        {
            // Si el punto de generación está demasiado cerca del jugador, mueve el punto de generación
            spawnPoint = player.position + spawnDirection * (spawnDistance + minDistanceFromPlayer);
        }

        Instantiate(asteroid, spawnPoint, Quaternion.identity, transform);
    }
}

