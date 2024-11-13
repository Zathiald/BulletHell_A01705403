using System.Collections;
using UnityEngine;

public class WhiplashShip : MonoBehaviour
{
    public Transform startPoint;    // Punto de inicio de la nave
    public Transform endPoint;      // Punto final de la nave
    public float moveSpeed = 5f;    // Velocidad de movimiento de la nave
    public GameObject bulletPrefab; // Prefab del proyectil
    public float fireRate = 0.5f;   // Intervalo de disparo en segundos
    public Transform[] firePoints;  // Puntos desde los cuales disparar

    private float t = 0f;           // Variable para interpolar entre los dos puntos

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
}
