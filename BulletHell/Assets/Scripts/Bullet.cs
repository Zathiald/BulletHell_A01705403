using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;
    public float timeDestroy = 3f;
    public float damage = 10f; // Daño que causa la bala

    void Start()
    {
        transform.Rotate(90f, 0f, 0f);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb != null) 
        {
            rb.velocity = transform.up * speed;
        }
        else
        {
            Debug.LogError("Rigidbody no encontrado en la bala.");
        }
        
        Destroy(gameObject, timeDestroy);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión del jugador con: " + other.gameObject.name);  // Ver qué objeto está colisionando
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Colisión con enemigo");  
            var enemy = other.GetComponent<IDamage>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Atacado Enemigo");
            }
            Destroy(gameObject); // Destruye la bala al impactar
        }
    }

}
