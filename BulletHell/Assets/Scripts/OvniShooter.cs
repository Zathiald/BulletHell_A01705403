using System.Collections;
using UnityEngine;

public class OvniShooter : MonoBehaviour
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

    void Start()
    {
        StartCoroutine(AutoShoot()); // Inicia la rutina de disparo
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
}

