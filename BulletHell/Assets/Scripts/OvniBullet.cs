using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvniBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;
    public float timeDestroy = 3f;
    public float damage = 10f; // Daño que causa la bala

    void Start()
    {
        // Inicializa el Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb != null) 
        {
            // Aplica la velocidad hacia adelante en función de la rotación del objeto (ya rotado en X)
            rb.velocity = transform.up * speed; // Usa 'up' para moverlo según la rotación ajustada
        }
        else
        {
            Debug.LogError("Rigidbody no encontrado en la bala.");
        }
        
        Destroy(gameObject, timeDestroy); // Destruye la bala después de 'timeDestroy' segundos
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión del enemigo con: " + other.gameObject.name);  // Ver qué objeto está colisionando
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colisión con jugador"); 
            var player = other.GetComponent<ShipController>();
            if (player != null)
            {
                player.TakeDamage(damage, Color.green);
                Debug.Log("Atacado Jugador");
            }
            Destroy(gameObject); // Destruye la bala al impactar
        }
    }
}

